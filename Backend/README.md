# melman-vr-backend

## How to Run the Server

1. Make sure you have Python installed on your machine.
2. Install the necessary packages with the following command:

    ```bash
    pip install flask pydantic
    ```

3. Save the script to a file, for example `server.py`.
4. Run the server with the command:

    ```bash
    python3 main.py
    ```

The server will be running on `http://X.Y.X.W:5000` and will accept POST requests at the `/frames` endpoint with a JSON body matching the `FrameDto` schema.

## Example JSON Payload

Here's an example of what the JSON payload should look like when sending a POST request to the `/frames` endpoint:

```json
{
    "hmd": {
        "position": {"x": 1.0, "y": 2.0, "z": 3.0},
        "rotation": {"x": 0.0, "y": 0.0, "z": 0.0}
    },
    "leftHand": {
        "position": {"x": 4.0, "y": 5.0, "z": 6.0},
        "rotation": {"x": 3.14, "y": 0.0, "z": 0.0}
    },
    "rightHand": {
        "position": {"x": 7.0, "y": 8.0, "z": 9.0},
        "rotation": {"x": 0.0, "y": 0.0, "z": 0.0}
    },
    "time": 123.456
}
```

You can use a tool like `curl` or Postman to send this payload to the server:

```bash
curl -X POST http://127.0.0.1:5000/frames -H "Content-Type: application/json" -d '{"hmd":{"position":{"x":1.0,"y":2.0,"z":3.0},"rotation":{"x":0.0,"y":0.0,"z":0.0}},"leftHand":{"position":{"x":4.0,"y":5.0,"z":6.0},"rotation":{"x":10.0,"y":20.0,"z":30.0}},"rightHand":{"position":{"x":7.0,"y":8.0,"z":9.0},"rotation":{"x":40.0,"y":50.0,"z":60.0}},"time":123.456}'
```

This will send the JSON data to the server, which will parse it into a `FrameDto` object and print it out.