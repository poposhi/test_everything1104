# coding=utf-8
# 需要讀取預測數值 並且寫出排程 
#讀txt 轉換成array 
import numpy as np

filename = "C:\\Users\\johnny\\source\\repos\\test_everything\\python_write.txt" # txt檔案和當前指令碼在同一目錄下，所以不用寫具體路徑
pos = []

with open(filename, 'r') as file_to_read:
    while True:
        lines = file_to_read.readline() # 整行讀取資料
        if not lines:
            break
        p_tmp = [float(i) for i in lines.split()] # 將整行資料分割處理，如果分割符是空格，括號裡就不用傳入引數，如果是逗號， 則傳入‘，'字元。
        pos.append(p_tmp)  # 新增新讀取的資料
pos = np.array(pos) # 將資料從list型別轉換為array型別。
print(pos)

#把array的每個值 寫入 成一行
f1 = open("C:\\Users\\johnny\\source\\repos\\test_everything\\python_write.txt",'w')
for i in pos:
    f1.write(str(i[0])+'\n')

f1.close()
