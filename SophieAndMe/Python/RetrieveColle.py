import pandas as pd
import  sqlite3

con_i = sqlite3.connect("C:\\Users\\Bastien\\source\\repos\\Sophieandme\\Sophieandme\\user_value.db")
cur_i = con_i.cursor()


df1 = pd.read_excel('Coll (3).xlsx')
L = []
fixed_col = ["Matiére",'Noms',"Jours","Heures","Salle"]
date_columns = df1.columns.difference(fixed_col)

result = []
GC = 7

for _, row in df1.iterrows():
    Matier = row["Matiére"]
    nom = row["Noms"]
    Jours = row["Jours"]
    Heure = row["Heures"]
    salle = row["Salle"]
    for date in date_columns:
        if row[date] == GC:
            L.append(date)
            result.append({
                "Nom":nom,
                "Date":date,
                "Jour":Jours,
                "heure":Heure,
                "salle":salle,
                "Matiére":Matier
            })

# result = {k: v for k,v in sorted(result.items(), key=lambda item:item())}
result = sorted(result, key=lambda d:d['Date'])
for i in result:
    nom = i["Nom"]
    date = i["Date"]
    Jours = i["Jour"]
    Heure = i["heure"]
    salle = i["salle"]
    Matier = i["Matiére"]
    Command = "INSERT INTO Colles (Nom,Date,heure,Jours,Salle,Matiére) VALUES (?,?,?,?,?,?)"
    val = (nom,date,Heure,Jours,salle,Matier)
    print(Command)
    print(val)
    # cur_i.execute(Command, val)
    # con_i.commit()
    print("1 record inserted, ID:", cur_i.lastrowid)
    print("###########################################################################")





