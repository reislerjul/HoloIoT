# Project Description
HoloIoT is a project aimed to help factory workers by combining augmented reality and IoT. It allows a user to identify an IoT device, query its telemetry, and visualize the data as holograms. In doing so, a worker can view telemetry live, hands-free, and at the location of the device. This is a drastic improvement over the status quo where workers either use hand-held devices or go to a control room to view telemetry.  

# Project Goals
The goal of this project was to create a HoloLens application with an end-to-end IoT solution that could empower factory workers. We aimed to build a cloud-based backend with an interactive UX and write documentation oriented towards developers that includes setup, code, and extensibility. While this project is self-containing, we expect it to serve as a basis for other IoT applications. Thus, the project is structured so that developers can easily modify and add to it. This extensibility can be in the form of different data sources, other methods to identify IoT devices, 3D models of devices, maintenance records, etc. 

# App Overview  
IoT devices send telemetry to a data source in the cloud. A user can identify a particular device using QR code scanning or text recognition. The user can then query the database and view the telemetry as a table or chart for the device. 

# App Features
1.	IoT Object Identification: system pulls frames from camera for up to 30 seconds 
  a.	QR Code Scanning: ZXing.Net C# library finds a QR code in the frames and decodes it to a device ID
  b.	Optical Character Recognition: Windows.Media.Ocr finds text in the frames and uses this as a device ID
2.	Anomaly Detection in IoT Data/Telemetry
  a.	System generates a list of devices with anomalies in the time range
  b.	Once a device is identified, system detects the number of anomalous data points the device measured in the time range
3.	 Audio-Based System Alerts 
  a.	System alerts the user of whether the object identification method succeeded or failed
  b.	Systems alerts the user of how many devices measured anomalies in the time range
  c.	System alerts the user of how many anomalies a specific device measured in the time range
4.	Input Gestures
  a.	Air-Tap: User can press buttons and check objects on the checklist
  b.	Voice Input: Used as an alternative to air-tapping for buttons and the checklist
5.	Entering Data through Gestures
  a.	Air-Tap and Number Pad: User air-taps a number, a number pad opens, user air-taps numbers on the pad to select them
  b.	Air-Tap and Hold: user air-taps or air-taps and holds a number, number increments
  c.	Dictation: user specifies the date using a regular expression 
6.	3D Visualization of IoT Data
  a.	Table: contains the minimum, maximum, average, and standard deviation of an attribute for the device and all devices in the factory
  b.	Chart: graphs the telemetry of a device’s attribute; the time is on the x-axis and the measurement on the y-axis  
7.	Live and Historic Data Query Mode 
  a.	Live Mode: the system queries the data source every 7 seconds and updates the display
  b.	Historic Mode: the system displays the telemetry from within a static time range

# Setup
See the documentation for a walkthrough on setting up the app and the cloud backend.

