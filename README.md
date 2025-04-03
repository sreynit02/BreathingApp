# Breathing App

## Purpose
This project aims to train people to take deep breaths by utilizing visualization as feedback. 

## How it works?
**Unity Application** - Initialised as WebSocket Server. Receives real-time data from connected clients and generates visualizations. 

**Python Script** - Initialised as WebSocket Client. Uses the GDX module to read data from the GoDirect Respiration Sensor. 
Sends data read from the sensor to the WebSocket server.

## Device needed: 
**HeadMounted Display** - Meta Quest 3

**GoDirect Respiration Belt**

**Laptop**

## Software Requirements
- websocket-sharp : https://github.com/sta/websocket-sharp
- gdx module from GDX: https://github.com/VernierST/godirect-examples/tree/main/python/gdx
- Unity v2022.3.35f1
