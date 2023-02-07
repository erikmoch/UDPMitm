# UDPMitm
This is a C# console application that implements a UDP Man-in-the-Middle (Mitm) proxy.
## How to Use
To use the application, open a command prompt and navigate to the directory where the executable file is located. Then run the following command:

##### UDPMitm.exe <REMOTE_IP_ADDRESS> <REMOTE_IP_PORT> <LOCAL_PORT>

REMOTE_IP_ADDRESS = IP address of the remote server;<br>
REMOTE_IP_PORT = Port of the remote server;<br>
LOCAL_PORT = Local port you want the application to listen on.

For example, if the remote server has IP address 54.39.163.216 and port 28015, and you want the application to listen on local port 8080, you would run the following command:

##### UDPMitm.exe 54.39.163.216 28015 8080
