## Prérequis

- .NET 7 SDK
- Docker + Docker Compose

## Lancement du projet

1. Cloner le dépôt
2. Démarrer MySQL avec Docker :

```bash
docker compose up -d
```

3. Appliquer les migrations :

```bash
dotnet ef database update
```

4. Lancer l'application :

```bash
dotnet run
```

5. Accéder à Swagger :

```
https://localhost:{port}/swagger
```

## Compte admin (seedé automatiquement)

- **Email** : admin@admin.com  
- **Mot de passe** : Admin123!

## Structure

- `Controllers/` : Endpoints HTTP
- `Services/` : Logique métier
- `Interfaces/` : Interfaces de services
- `Models/` : Entités EF Core
- `Dtos/` : Objets de transfert de données
- `Middlewares/` : Middleware d'erreurs RFC 7807
