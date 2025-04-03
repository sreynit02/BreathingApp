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

**Laptop** - Run Python code to read from sensor
<pre><code>
import asyncio
import websockets
import json
from gdx import gdx
import random

# Use this script with C# script UnityWebocketServer.cs
# Make sure your laptop and the device are connected to the same network

gdx = gdx.gdx()
#gdx.open(connection='ble', device_to_open='Device ID') #replace Device ID found on the the device
gdx.select_sensors([1]) #Reeads force data
gdx.start(1000)  # Adjust the rate to 1000ms (1 second)

async def send_data():

    uri = "ws://localhost:8000/data"

    async with websockets.connect(uri) as websocket:
        while True: 
            measurements = gdx.read()
            if measurements is not None:
                data = {"value": measurements[0]}
                try: 
                    await websocket.send(json.dumps(data))
                    print(f"Send data: {data}")
                except websockets.ConnectionClosed:
                    print("Connection Closed")
                    break 
            else:
                print("No Measurements available")
            await asyncio.sleep(1)

async def main():
    await send_data()

if __name__ == "__main__":
    # Use `asyncio.run()` only if no other event loop exists
    loop = asyncio.get_event_loop()
    loop.run_until_complete(main())
    
gdx.stop()
gdx.close()
  
</code></pre>


## Software Requirements
- websocket-sharp : https://github.com/sta/websocket-sharp
- gdx module from GDX: https://github.com/VernierST/godirect-examples/tree/main/python/gdx
- Unity v2022.3.35f1

