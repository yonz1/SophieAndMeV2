import datetime
import json
import sqlite3
import requests
import ast


con_i = sqlite3.connect("C:\\Users\\Bastien\\source\\repos\\Sophieandme\\Sophieandme\\data_restored.db")
cur_i = con_i.cursor()

con_f = sqlite3.connect("C:\\Users\\Bastien\\source\\repos\\Sophieandme\\Sophieandme\\user_value.db")
curf_f = con_f.cursor()

class util:
    def name(n):
        dico = L["fields"]
        for i in dico.keys():
            if int(n) == int(i):
                return dico[i]["value"]
        return "unkwnow"


def find_between(s, first, last):
    try:
        start = s.index( first ) + len( first )
        end = s.index(last,start)
        return s[start:end]
    except ValueError:
        return ""


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


url = "https://www.sophieand.me/quizzs#"
cookies = {"laravel_session" : "eyJpdiI6IkIzYklaZUhkYjZQc2VvbU1YYURRSFE9PSIsInZhbHVlIjoiRnQyR1NjcGRWXC9PTUh3K2hmSWRweERlTlVRRDBCdkNOTGU0UkwyVHM1ZTkyWGNzTCsxbUVFMjg3SEsybm5cL0hvcE1MK3VwMXY2U2tDck4wY3VtZnJaUT09IiwibWFjIjoiYTkyZjEyMGJmYjM4NjZmODc1NTRkMzY4NTM2MjgxMmNjMWVjMzU2ZDcyYzgxOWNjZDZlNWE1YTU1MWQ0M2ZjNiJ9"}
req = requests.get(url, cookies = cookies)
s = req.text
print(s)
start = "ReactDOM.render(React.createElement(window.StudentApp,"
end = "), document.getElementById('studentApp'));"
data = find_between(s, start,end)
data = json.loads(data)
update = replace_none(data)
data = json.dumps(update)
L = ast.literal_eval(data)
print(type(L))

idtot: list= []
dico = L["fields"]
print(dico)
try :
    for i in dico.keys():
        print(dico[i]["value"])
        command = "CREATE TABLE " + str(dico[i]["value"]) + " (id VARCHAR(255), name VARCHAR(255), question VARCHAR(255), reponse VARCHAR(255), image_question_url VARCHAR(255), image_answer_url VARCHAR(255) , difficulty VARCHAR(255), Marked VARCHAR(255))"
        cur_i.execute(command)
except Exception as e :
    print(e)

######################################## Permet d'avoir toute les questions d'un quizz

dico = L["quizzs"]
for i in dico.keys():
    idtot.append(i)


rep = 0
anciename = ""
val = {}
matierebyname = {}



for k in idtot:
    matlist = []
    name = dico[k]["name"]
    val = dico[k]["questions"]
    dico2 = {index: value for index, value in enumerate(val)}
    value = dico2.keys()
    for i in value:
        matiere = util.name(dico2[i]["question"]["field_id"])
        if matiere not in matlist:
            matlist.append(matiere)
    matierebyname.update({name: matlist})

for k in idtot:
    name = dico[k]["name"]
    val = dico[k]["questions"]
    dico2 = {index: value for index, value in enumerate(val)}
    value = dico2.keys()
    matiere = matierebyname[name]
    for y in matiere:
        rep = 0
        verifpresence = 'SELECT COUNT(*) from ' + y + ' WHERE name = "' + str(name) + '"'
        print(verifpresence)
        exist = cur_i.execute(verifpresence)
        count = str(exist.fetchall()).replace("(", "").replace(")", "").replace("[", "").replace("]", "").replace(",", "")
        if int(count) == 0:
          value =  datetime.datetime.now()
          Command = "INSERT INTO Date (Name,Inserted) VALUES (?,?)"
          val = (name,value)
          curf_f.execute(Command, val)
          con_f.commit()

          for i in value:
            question = dico2[i]["question"]["question"].encode('utf-8', 'replace').decode()
            answer = dico2[i]["question"]["answer"].encode('utf-8', 'replace').decode()
            image_question = dico2[i]["question"]["image_question_url"]
            image_answer = dico2[i]["question"]["image_answer_url"]
            difficulty = dico2[i]["question"]["difficulty"]
            if anciename == "":
                anciename = name
            if anciename == name:
                rep += 1
            else :
                rep = 1
            # print(name)
            # print(rep)
            # print(matiere)
            # print(question)
            # print("answer : ")
            # print(answer)
            # print("###########################################################################")
            anciename = name
            Marked = ""
            Command = "INSERT INTO " + y + " (id,name,question,reponse,image_question_url,image_answer_url,difficulty,Marked) VALUES (?,?,?,?,?,?,?,?)"
            val = (str(rep),name,question,answer,image_question,image_answer,difficulty,Marked)
            print(Command)
            print(val)
            cur_i.execute(Command,val)
            con_i.commit()
            print("1 record inserted, ID:", cur_i.lastrowid)
            print("###########################################################################")



##### Supprimer toute les donnée entourant la valeurs
####" Enlever la valeur ), document.getElementById('studentApp')); });
####### Supprimer les espaces entre les valeurs 
##### Remplacer tout les null par "null", tout les false par "false" et tout les tru par "true"

















