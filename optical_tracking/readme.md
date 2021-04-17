# Optical Tracking

![Build and Touch in Action](../img/mediapipe-optical-tracking.png)

This folder contains the optical tracking script written in python. The application tries to connect to an tcp socket upon start and accesses the users webcam as an input device.
The hands landmarks get acquired through the use of [Google's mediapipe library](https://google.github.io/mediapipe/) and get set over tcp through the use of [Googles Protocol Buffers (protobuf)](https://developers.google.com/protocol-buffers/).
As the data is sent via protobuf it is possible to use any language that is supported by it on the receiving end.

## Building the Project
Before you start using any of the projects component, you first need to compile the protobuf file for your desired language.

To compile the protobuf files you need `protoc` in version 3 or higher. The current version can be found on https://github.com/protocolbuffers/protobuf.


```bash
protoc handdata.proto --csharp_out=../unity_sample/src/build-and-touch/Assets/Scripts --python_out=src
```

To build the protobuf files for another languages see: https://developers.google.com/protocol-buffers/docs/tutorials.

# Install the Requirements

1. Optionally create a python virtual environment (e.g. with `virtualenv -p python3 <name>`) and activate it (e.g. `source <name>/bin/activate`).
2. Install all requirements using `pip install -r requirements.txt`
3. Optionally configure the python scripts to your needs.
4. Build the protobuf file if not already done.
5. Execute main.py (NOTICE: The script tries to immediately connect to the IP-Address and port specified in `connection.py`. If nothing listens on this socket the application will close.) 


