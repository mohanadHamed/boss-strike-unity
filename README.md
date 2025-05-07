
# boss-strike-unity

**Boss Strike** is a 3D local co-op action game built in Unity. Two-player team up to battle a powerful boss that uses multiple attack patterns like fire flame, rocket strikes, and eagle strikes.

## 🔥 Game Features

- Local co-op with support for two players
- A challenging boss with:
  - Fire Flame attacks
  - Rocket homing attacks
  - Eagle Strike dive attacks
- Humanoid animated characters with run/idle/hit animation states
- Scene loading with progress feedback
- Game data saving and loading (player names, characters, sound settings, leaderboard)

## 🎮 Controls

| Player | Move            | Run/Stop           |
|--------|---------------- |--------------------|
| P1     | WASD            | Hold/Release       |
| P2     | NUMPAD(8/4/6/2) | Hold/Release       |

## 💾 Saving System

- Saves game data to a JSON file
- Stores:
  - Player names and selected characters
  - Sound settings
  - Leaderboard scores

## 🧠 Boss AI

The boss selects a random player and:
- Moves toward them
- Uses different attacks based on range
- Rotates and locks position during attack
- Uses raycast-based fire beams and rockets

## 🛠 Tech Stack

- Unity 6.0 LTS
- C#
- Supports PC (Windows)

## 📂 Project Structure

```
Assets/
├── Scripts/
│   ├── Game/
│   ├── MainMenu/
│   ├── SaveSystem/
│   └── Utilities/
├── Prefabs/
├── Scenes/
├── Resources/
├── Sounds/
├── Animations/
└── Materials/
```

## 🚀 Getting Started

1. Clone the repo
2. Open the project in Unity (project Unity versino 6000.048f1)
3. Open the scene `MainMenu`
4. Hit Play!

## Demo
https://github.com/user-attachments/assets/e6cb252c-1a79-4e44-9fbf-80b40b4f37f0



## Screenshots
![image](https://github.com/user-attachments/assets/9de5ed63-cfcc-4fb1-941b-e18b1d44b4fe)
![image](https://github.com/user-attachments/assets/9541823a-f4ed-48cf-a4f5-f8911f0a71d5)
![image](https://github.com/user-attachments/assets/4edd5378-7bbf-4b68-b245-c49a9d846524)
![image](https://github.com/user-attachments/assets/56d2666b-331e-4cbc-bc76-e2fb381b057e)
![image](https://github.com/user-attachments/assets/0f42ec64-707c-4bf9-9703-bb219e76a06b)

