import ast
import json
import sqlite3
import requests
from Cython.Compiler.Code import read_utilities_hook
from bs4 import BeautifulSoup
from colorama import  Fore,Style


FieldsDict = {}
CoursesDict = {}
LevelsDict = {}
OldQuestion = []
z= 0
con_i = sqlite3.connect("C:\\Users\\Bastien\\RiderProjects\\SophieAndMe\\SophieAndMe\\Database\\PublicDB.db")
cur_i = con_i.cursor()

def find_between(s, first, last):
    try:
        start = s.index( first ) + len( first )
        end = s.index(last,start)
        return s[start:end]
    except ValueError:
        return ""
def getCookie(val):
    start = "laravel_session="
    end = " "
    return find_between(str(val),start,end)
def replace_none(obj):
    if isinstance(obj, dict):
        return {k: replace_none(v) for k,v in obj.items()}
    elif isinstance(obj, list):
        return [replace_none(elem) for elem in obj]
    elif obj is False:
        return "false"
    elif obj is None:
        return "null"
    elif obj is True:
        return "true"
    else:
        return obj
def Setup():
    for i in FieldsDictTemp:
        FieldsDict[i] = FieldsDictTemp[i]["value"]
    for i in CoursesDictTemp:
        CoursesDict[i] = CoursesDictTemp[i]["value"]
    for i in LevelsDictTemp:
        LevelsDict[i] = LevelsDictTemp[i]["value"]
    print(FieldsDict)
    print(CoursesDict)
    print(LevelsDict)



def GetInfoLevel(number):
    return LevelsDict[number]
def GetInfoFields(number):
    return FieldsDict[number]
def GetInfoCourses(number):
    return  CoursesDict[number]




email = "theophilegernin@gmail.com"
password = "DRAgon.12345"


url = "https://www.sophieand.me/login"
session = requests.Session()
rep = session.get(url)
cookies = {"email" : email,"password":password}
headers = {'Referer': url}
post_response = session.post(url,data=cookies,headers=headers)
laravel_session = getCookie(post_response.cookies)
cookies = {'laravel_session': laravel_session}
url = "https://www.sophieand.me/questions"
req = requests.get(url, cookies=cookies)
s = req.text
start = "ReactDOM.render(React.createElement(window.QuestionsLibrary,"
end = "), document.getElementById('questionLibraryContainer'));"
data = find_between(s, start,end)
data = json.loads(data)
update = replace_none(data)
data = json.dumps(update)
L = ast.literal_eval(data)
FieldsDictTemp = L["fields"]
CoursesDictTemp = L["courses"]
LevelsDictTemp = L["levels"]
Setup()

y = 1
j = True
ListData = []
next_page = "https://www.sophieand.me/api/v1/questions?exclude_already_owned=true&search="

while next_page != "null":
        url = next_page
        req = requests.get(url, cookies=cookies)
        s = req.text
        data = json.loads(s)
        update = replace_none(data)
        data = json.dumps(update)
        L = ast.literal_eval(data)
        next_page = L["next_page"]
        ListData += L["data"]
        print(next_page)
        if L["data"][0]["question"] in OldQuestion:
            print(Fore.RED + str(L["data"][0]["question"].encode('utf-8', 'replace').decode().replace("\"","'")))
            z += 1
        else:
            print(Fore.GREEN + str(L["data"][0]["question"].encode('utf-8', 'replace').decode().replace("\"","'")))
            OldQuestion.append(L["data"][0]["question"])
            ListData += L["data"]





for i in FieldsDict.values():
        try:
            print(i)
            command = "CREATE TABLE " + str(i) + " (level_id VARCHAR(255), course VARCHAR(255), question VARCHAR(255), reponse VARCHAR(255), image_question_url VARCHAR(255), image_answer_url VARCHAR(255) , difficulty VARCHAR(255))"
            print(command)
            cur_i.execute(command)
        except Exception as e:
            print(e)

for i in ListData:
    try:
        fields = GetInfoFields(i["field_id"])
        question = i["question"].encode('utf-8', 'replace').decode().replace("\"","'")
        reponse = i["answer"].encode('utf-8', 'replace').decode().replace("\"","'")
        image_question_url = i["image_question_url"]
        image_answer_url = i["image_answer_url"]
        level_id = GetInfoLevel(i["level_id"])
        course = GetInfoCourses(i["course_id"])
        difficulty = i["difficulty"]
        verifpresence = "SELECT COUNT(*) from " + fields + " WHERE level_id = \"" + level_id + "\" AND question = \"" + question + "\""
        exist = cur_i.execute(verifpresence)
        count = str(exist.fetchall()).replace("(", "").replace(")", "").replace("[", "").replace("]", "").replace(",", "")
        if int(count) == 0:
            Command = "INSERT INTO " + fields + " (level_id,course,question,reponse,image_question_url,image_answer_url,difficulty) VALUES (?,?,?,?,?,?,?)"
            val = (level_id, course, question, reponse, image_question_url, image_answer_url, difficulty)
            print(Command)
            print(val)
            cur_i.execute(Command, val)
            con_i.commit()
            print("1 record inserted, ID:", cur_i.lastrowid)
            print("###########################################################################")
    except Exception as e:
        print(verifpresence)
        print(e)



ListDel = ["CM2","CM1","2nd","3e","6e","5e","4e","1er Sti2d"]
for i in ListDel:
    for y in FieldsDict.values():
        Command = "DELETE FROM " + y + " WHERE level_id = \"" + i + "\""
        print(Command)
        cur_i.execute(Command)
        con_i.commit()

############################ Algo pour la création des tables

print(z)

