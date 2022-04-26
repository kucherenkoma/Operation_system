import threading
import time
import hashlib
from itertools import product

num_of_threads = int(input('Введите количество потоков: '))
for_threads = list(zip(*[iter(product('abcdefghijklmnopqrstuvwxyz', repeat=5))]*(26**5//num_of_threads)))
HASH = input('Введите хэш: ')
HASH = HASH.encode().decode()


def decrypt(num):
    for i in range(len(for_threads[num])):
        if HASH == hashlib.sha256(''.join(for_threads[num][i]).encode()).hexdigest():
            print('Пароль:', ''.join(for_threads[num][i]))
            global end
            end = time.time()
            break


thread_list = []
start = time.time()
for i in range(num_of_threads):
    thr = threading.Thread(target=decrypt, name='thread{}'.format(i), args=(i,))
    thr.start()
    thread_list.append(thr)
for i in thread_list:
    i.join()             # ждем завершения всех потоков
print('Время: {}'.format(end-start))
