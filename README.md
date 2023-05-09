# PedroDB

## C# Interface Example

> **Nota:** Todas as classes estão no namespace `PedroDB`

### Inicializações
Criar um objeto da engine PedroDB:
```cs
EngineConfiguration config = new() {
  DatabasePath = Environment.CurrentDirectory + "/db"
};
PedroEngine engine = new(config);
```

Criar um banco de dados e pegar uma instância dele: 
```cs
engine.AddDatabase("library");
Database db = engine.GetDatabase("library");
```

Criar uma coleção no banco de dados e pegar uma instância dele
```cs
db.AddCollection("users");
Collection<Person> people = db.GetCollection<Person>("users");
```

### Usando na prática

Após criar e pegar uma coleção, pode-se adicionar itens à ela, exemplo:
```cs
// people é Collection<Person>
people.Add(new Person("Pedro", 18));
people.Add(new Person("John", 20));
people.Add(new Person("Jane", 21));
```

E quando necessário, pode ser usado um loop normal para procurar itens:
```cs
foreach(var p in people) {
  Console.WriteLine(p.Name);
  Console.WriteLine(p.Age);
}
```
