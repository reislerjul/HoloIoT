# Project Description
HoloIoT is a project aimed to help factory workers by combining augmented reality and IoT. It allows a user to identify an IoT device, query its telemetry, and visualize the data as holograms. In doing so, a worker can view telemetry live, hands-free, and at the location of the device. This is a drastic improvement over the status quo where workers either use hand-held devices or go to a control room to view telemetry.  

# Project Goals
The goal of this project was to create a HoloLens application with an end-to-end IoT solution that could empower factory workers. We aimed to build a cloud-based backend with an interactive UX and write documentation oriented towards developers that includes setup, code, and extensibility. While this project is self-containing, we expect it to serve as a basis for other IoT applications. Thus, the project is structured so that developers can easily modify and add to it. This extensibility can be in the form of different data sources, other methods to identify IoT devices, 3D models of devices, maintenance records, etc. 

# App Overview  
IoT devices send telemetry to a data source in the cloud. A user can identify a particular device using QR code scanning or text recognition. The user can then query the database and view the telemetry as a table or chart for the device. 
