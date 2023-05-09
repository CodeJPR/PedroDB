# PedroDB

## C# Interface Example

> **Nota:** Todas as classes estão no namespace `PedroDB`

### Inicializações

Primeiramente, deve-se criar um objeto representando a engine PedroDB. Note que
o DatabasePath é o caminho onde o sistema ficará armazenado. Exemplo: 
```cs
EngineConfiguration config = new() {
  DatabasePath = Environment.CurrentDirectory + "/db"
};
PedroEngine engine = new(config);
```

Para manipular bancos de dados, podem ser usados os métodos: 
* `void AddDatabase(string name)`
* `void RemoveDatabase(string name)`
* `Database GetDatabase(string name)`

Segue um exemplo: 
```cs
engine.AddDatabase("library");
Database db = engine.GetDatabase("library");
```

Similarmente, existem tais métodos para gerenciar coleções dentro de um banco de dados: 
* `void AddCollection(string name)`
* `void RemoveCollection(string name)`
* `Collection<T> GetCollection<T>(string name)`

Segue um exemplo de como criar e pegar uma coleção de pessoas:
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
