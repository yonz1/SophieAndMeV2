import sqlite3
import requests
import re
from bs4 import BeautifulSoup

from PLus.test import username

con_f = sqlite3.connect("C:\\Users\\Bastien\\source\\repos\\Sophieandme\\Sophieandme\\user_value.db")
curf_f = con_f.cursor()

L = []
def remove_items(test_list, item):
    res = [i for i in test_list if i != item]
    return res

def miseneforme(text):
    ret = re.sub(r'^[ \t]+', '', text, flags=re.MULTILINE)
    ret = ret.replace("<th>","").replace("</th>","").replace("<span","").replace("</span>","")
    ret = ret.replace("<td></td>\n","Pas de colle").replace("</td>","").replace("<td>","").replace("\n\n","\n").replace("\n\n\n","\n").replace(">",";")
    ret = re.split(r'[\n]', ret)
    ret = remove_items(ret,"")
    return ret

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


def find_between2(s, first, last):
    try:
        start = s.index( first ) + len( first )
        end = s.index(last,start)
        return s[start:end]
    except ValueError:
        return ""

def find_between(s, first, last):
    L = []
    S = []
    for i in range (len(s)):
        if (s[i:i+len(first)] == first):
            findex = i+len(first)
        elif (s[i:i+len(last)] == last):
            lindex = i
            value = s[findex:lindex].replace("title = ","").replace("\"","")
            L.append(value)
    return L



def getCookie(val):
    start = "sessionid="
    end = " "
    return find_between2(str(val),start,end)

username = ""
password = ""

url = "https://eiffel-dijon.prepas-plus.fr/connexion?next=/"
session = requests.Session()
rep = session.get(url)
soup = BeautifulSoup(rep.text, 'html.parser')
csrf_token = soup.find('input',{'name':'csrfmiddlewaretoken'}).get('value')
cookies = {"username" : username,"password":password,"next":"None",'csrfmiddlewaretoken':csrf_token}
headers = {'Referer': url}
post_response = session.post(url,data=cookies,headers=headers)
sessionid = getCookie(post_response.cookies)
url = "https://eiffel-dijon.prepas-plus.fr/colles/mes_notes"
cookies = {'sessionid': sessionid}
req = requests.get(url, cookies = cookies)
s = req.text

if (s == ""):
    print("Aucun compte n'est enregistrées avec ces identifiants,veuillez réesayer")


start = "<tr>"
end = "</tr>"
data = find_between(s, start,end)
# data = re.sub(r'^[ \t]+', '', data, flags=re.MULTILINE).replace(" ","")
val = []
for i in data:
  print(i)
  value = miseneforme(i)
  print(value)
  val.append(value)

print("Matiére : ", val[0])
for j in val[1:]:
    verifpresence = 'SELECT COUNT(*) from PLUS WHERE Semaine = "' + j[0] + '"'
    print(verifpresence)
    exist = curf_f.execute(verifpresence)
    count = str(exist.fetchall()).replace("(", "").replace(")", "").replace("[", "").replace("]", "").replace(",", "")
    if int(count) == 0:
        Command = "INSERT INTO Plus (Semaine,Anglais,Français,Maths,Physique,SI) VALUES (?,?,?,?,?,?)"
        val = (j[0],j[1],j[2],j[3],j[4],j[5])
        print(Command)
        print(val)
        curf_f.execute(Command, val)
        con_f.commit()
        print("1 record inserted, ID:", curf_f.lastrowid)
        print("###########################################################################")










