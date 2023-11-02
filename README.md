# Adaptive Intensity System with Hyperate Integration

Welcome to our Adaptive Intensity System with real-time integration to the Hyperate API. This system is designed to adjust game or simulation intensity based on real-time heart rate data, providing an immersive experience for the user.

## Overview
The system comprises two primary components:

### 1. `hyperateSocket`:
This component is responsible for:
- Establishing a WebSocket connection to Hyperate's servers.
- Receiving real-time heart rate updates from Hyperate.
- Updating our Intensity Manager (`AdaptiveIntensity`) with the received BPM data.

**Key Features**:
- **WebSocket Token**: Before using the system, you must obtain a token from Hyperate. [Get it here](https://www.hyperate.io/api).
- **Heartbeat Mechanism**: Ensures the connection remains active and stable.
- **Error Handling**: It provides feedback in case of any discrepancies or issues with the WebSocket.

### 2. `AdaptiveIntensity`:
This component adjusts the intensity based on the heart rate data received:
- Computes the average BPM from a history of received BPMs.
- Adjusts the intensity dynamically based on the BPM difference.
- Interpolates the BPM difference into a normalized intensity value.

**Key Features**:
- **Interpolation Method**: Currently, it uses a linear interpolation method. More methods can be added as per future requirements.
- **History Length**: Determines how many of the most recent BPM values are considered for average BPM computation.
- **Intensity Update Interval**: Configurable duration to update the intensity average.

## Getting Started
- Ensure you have a WebSocket token from Hyperate and plug it into the `hyperateSocket` component.
- Attach the `AdaptiveIntensity` script to a GameObject. This object will handle the intensity adjustments.
- Attach the `hyperateSocket` script to a separate GameObject and reference the `AdaptiveIntensity` object.
- Configure parameters like max/min BPM, history length, etc., according to your requirements.
- Run the simulation or game. Heart rate updates will be received from Hyperate, and intensity adjustments will be made accordingly.

## Additional Notes
- Ensure you have an active and stable internet connection, as the system heavily relies on real-time WebSocket communication.
- This system is designed for Unity, and certain platform-specific conditions (like WebGL) have been considered. Ensure you test on your target platform before deployment.
