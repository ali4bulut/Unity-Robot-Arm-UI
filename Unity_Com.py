import socket
import time
import numpy as np
import RobotKol
import serial
import serial.tools.list_ports

port_name = "None"
arduino = False

ports = serial.tools.list_ports.comports()
if ports:
    for i in ports:
        temp = str(i).split(" ")
        if "CH340" in temp:
            port_name = temp[0]

print(port_name)

try:
    host, port = "127.0.0.1", 25001
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host, port))

    acilar = [0, 0, 0, 0, 0, 0]
    a1, a2, a3, a4, a5, a6 = 330, 40, 445, 40, 440, 164.2
    PT = [[0, 0, a1, 0],
          [np.pi / 2, a2, 0, np.pi / 2],
          [0, a3, 0, 0],
          [np.pi / 2, a4, a5, 0],
          [-np.pi / 2, 0, 0, 0],
          [np.pi / 2, 0, a6, 0]]
    uzuvlar = [a1, a2, a3, a4, a5, a6]
    robot = RobotKol.RobotKol(PT, uzuvlar)

    if port_name != "None":
        ser = serial.Serial(port_name, 9600)
        arduino = True
    else:
        print("Arduino Bulunamadı!!!")
        arduino = False

    counter = 32  # Below 32 everything in ASCII is gibberish
    while True:

        time.sleep(0.2)  # sleep 0.5 sec

        receivedData = sock.recv(1024).decode("utf-8")  # receive data in Byte from C#, and converting it to String
        f = list(map(float, receivedData.split("|")))
        # print(f)

        if f:
            ik = robot.IK(f[3] * np.pi / 180, f[4] * np.pi / 180, f[5] * np.pi / 180, f[0], f[1], f[2])

            for i in range(6):
                acilar[i] = ik[i] * (180 / np.pi)

            posString = ','.join(map(str, acilar))  # Converting Vector3 to a string, example "0,0,0"
            sock.sendall(posString.encode("utf-8"))  # Converting string to Byte, and sending it to C#
            # print(posString)

        if arduino:
            counter += 1
            ser.write(
                str(chr(counter)).encode("utf-8"))  # Convert the decimal number to ASCII then send it to the Arduino
            print(ser.readline().decode("ascii"))  # Read the newest output from the Arduino

            if counter == 99:
                counter = 32

except socket.error as msg:
    print("Caught exception socket.error : %s" % msg)

    # while True:
    #     time.sleep(0.5)  # sleep 0.5 sec
    #
    #     receivedData = sock.recv(1024).decode("UTF-8")  # receive data in Byte from C#, and converting it to String
    #     # print(receivedData, end="\n")
    #     f = list(map(float, receivedData.split("|")))
    #     print(f)
    #
    #     ik = robot.IK(f[3] * np.pi / 180, f[4] * np.pi / 180, f[5] * np.pi / 180, f[0], f[1], f[2])
    #
    #     for i in range(len(ik)):
    #         acilar[i] = ik[i] * (180 / np.pi)
    #
    #     posString = ','.join(map(str, acilar))  # Converting Vector3 to a string, example "0,0,0"
    #     print(posString)
    #     sock.sendall(posString.encode("UTF-8"))  # Converting string to Byte, and sending it to C#
