# Half Chess Game - Client

## Overview
This repository contains the client-side implementation for the Half Chess Game project. The client is responsible for managing the frontend and handling player interactions with the game. It communicates with the server to register players, start games, and query game statistics.

The client uses a 4x8 chessboard layout for the Half Chess game, where only the right half of a standard chessboard is used. Players interact with the game and the server to register, play, and track game data.

## Features

### Game Rules
1. **Board Layout**: The game board is 4 columns wide and 8 rows long. The right half of a standard chessboard is used, which includes:
   - Kings, Rooks, Bishops, and Knights are placed as usual for both sides, but only in the right half of the board.
   - Pawns are placed above each of the corresponding pieces.

2. **Piece Movement**:
   - **Pawn**: Pawns can move not only forward but also one square horizontally to the left or right (if the square is empty and no other pieces block the movement). This horizontal movement is for strategic purposes, not for capturing opponents' pieces.
   - **Time Limit**: Each player has a limited time to make a move; if the time expires, they lose the game.
   - **Standard Chess Rules**: All other movements follow standard chess rules.

3. **Game Flow**:
   - Players play against a server, which handles game management and centralizes the database.
   - Multiple games can occur simultaneously, and the server ensures no interaction between different player games.
  
### Web Interface
The client interacts with the web interface to query game statistics. This is done through the server.

### Player Registration and Gameplay
1. **Player Registration**: A player must register on the website before playing, and the registration must be successful.
2. **Game Start**: The game will display the player's information upon starting the game.
3. **Game Data Recording**: Each game will record details such as the players involved, the start time, game duration, and any other relevant information.
4. **Move Simulation**: The game generates valid moves for the player, without needing an algorithm to calculate the best move.

### Game Replay
Players can replay past games:
1. All games played by the player are stored and can be retrieved later.
2. Games are stored locally for each player, and the game can be selected from a list of past games.

