import openpyxl as openpyxl
import sqlite3
import re
import datetime


# Charger le fichier Excel avec openpyxl

con_i = sqlite3.connect("C:\\Users\\Bastien\\RiderProjects\\SophieAndMe\\SophieAndMe\\Database\\EDT.db")
cur_i = con_i.cursor()


def Init():
    Command = """CREATE TABLE "Vacance" (
	"DYear"	VARCHAR(255),
	"FYear"	VARCHAR(255),
	"Id"	INTEGER,
	"DMonth"	VARCHAR(255),
	"FMonth"	VARCHAR(255),
	"DDays"	VARCHAR(255),
	"FDays"	VARCHAR(255)
    )"""
    cur_i.execute(Command)
    Command = """CREATE TABLE "DSPlanning" (
	"Mat"	VARCHAR(255),
	"Date"	VARCHAR(255)
    )"""
    cur_i.execute(Command)




wb_obj = openpyxl.load_workbook('Livre 5.xlsx')
sheet_obj = wb_obj.active
Row = sheet_obj.max_row
Column = sheet_obj.max_column

for merged_range in (list(sheet_obj.merged_cells.ranges)):
    sheet_obj.unmerge_cells((str(merged_range)))
    L = str(merged_range).split(":")
    min_col, min_row, max_col, max_row = merged_range.bounds
    value = sheet_obj.cell(row=min_row, column=min_col).value
    if max_row-min_row > 1:
        for n in range(min_row+1,max_row):
            L.append(L[0][0]+str(n))
    for i in L:
        sheet_obj[i] = value


DS = []

for i in range (3,Column+1):
    Mat = sheet_obj.cell(row=1,column=i).value
    Act = sheet_obj.cell(row=2,column=i).value
    if (Act == "DS"):
        for y in range(3,Row+1):
            if (sheet_obj.cell(row=y,column=i).value == "x") or (sheet_obj.cell(row=y,column=i).value == "X"):
                # print(sheet_obj.cell(row=y,column=2).value," - ",Mat)
                date = sheet_obj.cell(row=y,column=2).value
                DS.append(date.strftime("%d/%m/%Y") + " - " + Mat)


Sem = []
DebutL = []
FinL = []
ValT = False
for i in range (4,Row+1):
    TypeSem = str(sheet_obj.cell(row=i,column=3).value)
    # print(TypeSem)

    if (not ValT and TypeSem == "None"):
        Debut = sheet_obj.cell(row=i,column=2).value
        DebutL.append(Debut)
        # print("Debut",str(sheet_obj.cell(row=i,column=2).value))
        ValT = True
    elif (ValT and not TypeSem == "None"):
        Fin = sheet_obj.cell(row=i-1,column=2).value
        FinL.append(Fin)
        # print("Fin",str(sheet_obj.cell(row=i,column=2).value))
        ValT = False

DS.sort()
for i in DS:
    i = i.replace(" ","").split("-")
    Command = ("INSERT INTO DSPlanning (Mat,Date) VALUES (?,?)")
    val = (i[0],i[1] )
    print(Command)
    print(val)
    cur_i.execute(Command, val)
    con_i.commit()


del DebutL[len(DebutL)-1]
for i in range(len(DebutL)):
    # print(DebutL[i].strftime("%x")," - ", FinL[i].strftime("%x"))
    Command = ("INSERT INTO Vacance (DYear,FYear,DMonth,FMonth,DDays,FDays) VALUES (?,?,?,?,?,?)")
    val = (DebutL[i].year,FinL[i].year,DebutL[i].month,FinL[i].month,DebutL[i].day,FinL[i].day)
    print(Command)
    print(val)
    cur_i.execute(Command, val)
    con_i.commit()
