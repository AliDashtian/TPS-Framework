A scalable, decoupled Third-Person Shooter (TPS) framework built in Unity. This project demonstrates a Scriptable Object Architecture Pattern (SOAP) approach to game development, prioritizing modularity, testability, and clean code.

It serves as the core engine for my current WIP mobile game, but is designed to be a generic foundation for any TPS project.

🌟 Key Features
🧠 Core Architecture
Finite State Machine (FSM): A robust, class-based State Machine for Character and AI logic (Idle, Fire, Reload, Dead, Swap).

Event-Driven System: Zero-dependency communication between systems using GameEvent ScriptableObjects.

Runtime Sets: managing lists of objects (Enemies, Players) at runtime without using Singletons.

Variable Architecture: Shared data types (FloatVariable, IntVariable) that allow data to be injected into prefabs independently of the scene.

🔫 Combat & Ballistics
Strategy-Based Weapon System: Supports RaycastWeapon, ProjectileWeapon, and PretendWeapon (for AI) logic via inheritance.

Locational Damage: BodyPartHitBox system allows for different damage multipliers based on the hit area (e.g., Headshots deal 2x damage).

Physical Material Response: Different VFX and SFX played based on surface types (Concrete, Flesh, Wood).

Dynamic Inventory: 2-slot weapon system with runtime swapping and drop/pickup logic.

🦾 Character Systems
Ragdoll Physics: Integrated RagdollActivator that seamlessly transitions from Animator control to Physics simulation upon death.

Stance System: Support for Standing, Crouching, Prone, and Cover states.

Character Switcher: Hot-swapping between multiple playable characters at runtime using PlayerRuntimeSet.

🛠 Architecture Overview
1. The Finite State Machine (FSM)
Character logic is broken down into small, reusable, class-based states rather than massive switch statements.

IState Interface: Defines the contract (Enter, Update, FixedUpdate, Exit).

CharacterState: Abstract base class handling common logic (Animation hashing, Input reading).

Workflow: The ActionFireState handles firing logic and automatically transitions back to ActionIdleState when ammo runs out, keeping the PlayerCharacter class clean of logic.

2. Scriptable Object Architecture (SOAP)
This project aggressively removes hard dependencies (like GameManager.Instance).

Event Channels: The UI doesn't know the Player exists. It simply listens to OnWeaponFired or OnHealthChanged events triggered by the system.

Atomic Variables: Shared data types (FloatVariable, IntVariable) allow systems to share state (like Player Health) without direct reference, enabling modular testing.

3. Singleton-Free Object Management (Runtime Sets)
Instead of using FindObjectOfType or static Singletons (which are hard to test), objects register themselves to ScriptableObject-based Lists at runtime.

How it works: When an Enemy spawns, it adds itself to the EnemyRuntimeSet asset.

The Benefit: The EnemyManager or WinLoseManager simply iterates over this Asset list to check if all enemies are dead. This allows logic to run even if the scene structure changes completely.

📦 Dependencies
This framework relies on the following packages:

UniTask: For efficient asynchronous scene loading and delays.

Cinemachine: For advanced camera control and screen shake.

Input System (New): For handling multi-platform inputs.

Animation Rigging: For procedural aiming and stance adjustments.
