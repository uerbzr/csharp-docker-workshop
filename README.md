# C# Docker Workshop

Here is an example of a WepApi project that has been Dockerized using Docker Compose.


## Docker Compose

- Note the **docker-compose** project which was created by:
	* In the WebFrontEnd project, choose Add > Container Orchestrator Support. The Docker Support Options dialog appears.
	* Choose Docker Compose.
	* Choose your Target OS, for example, Linux.  

-See here for [Microsoft Documentation](https://learn.microsoft.com/en-us/visualstudio/containers/tutorial-multicontainer?view=vs-2022)

## Dependencies:
```
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.InMemory
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.VisualStudio.Azure.Containers.Tools.Targets
Install-Package NpgSql.EntityFrameworkCore.PostgreSql
```

## Observations
- Note MigrationRunner class which runs programatically.  Necessary for when the application is started.   in Docker to generate the relevant tables in the database.
- Note the **Anonymous Object e.g.** 
```cs
var anonymous = new { Make=entity.Make, Model=entity.Model };
```
- Generic ```Repository<T>``` Layer
- Note the ```workshop.wwwapi.http``` file.  When the project is running, this enables you to test endpoints easily!
