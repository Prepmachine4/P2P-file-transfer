import json
import socket
import threading

host_list = []


def receiver():
    so = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    host_name = socket.gethostname()
    ip=socket.gethostbyname(host_name)
    so.bind((ip, 5000))
    while True:
        data, addr = so.recvfrom(1024)
        print('Recv:', data, addr)
        if data.decode() == 'login':
            host_list.append(addr[0])
            send('hosts'+json.dumps(host_list),addr[0])
            # 通知其他小程序刷新列表
            for i in range(len(host_list)):
                send('hosts'+json.dumps(host_list),host_list[i])
        elif data.decode() == 'logout':
            host_list.remove(addr[0])
            send('hosts' + json.dumps(host_list), addr[0])
            # 通知其他小程序刷新列表
            for i in range(len(host_list)):
                send('hosts' + json.dumps(host_list), host_list[i])



def send(data, ip):
    so = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    so.sendto(data.encode('UTF-8'), (ip, 5000))


if __name__ == '__main__':
    thread1 = threading.Thread(target=receiver)
    thread1.start()
