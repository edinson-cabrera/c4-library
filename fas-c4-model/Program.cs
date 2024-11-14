using Structurizr;
using Structurizr.Api;

namespace library_management_system
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDiagrams();
        }

        static void CreateDiagrams()
        {
            const long workspaceId = 97616;
            const string apiKey = "c823a3f6-7855-4aee-81ee-c41f2f9ff9e6";
            const string apiSecret = "fac64960-964c-4f82-bb90-a95c0ce9e8a1";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Library Management System", "Sistema para la gestión de libros y préstamos en una biblioteca.");
            Model model = workspace.Model;
            ViewSet viewSet = workspace.Views;

            // Diagrama de Contexto
            SoftwareSystem librarySystem = model.AddSoftwareSystem("Library Management System", "Sistema central de gestión de la biblioteca.");
            SoftwareSystem notificationService = model.AddSoftwareSystem("Notification Service", "Envía notificaciones push a los usuarios.");
            SoftwareSystem authenticationService = model.AddSoftwareSystem("Authentication Service", "Permite el registro y autenticación de usuarios.");

            Person librarian = model.AddPerson("Librarian", "Administra libros y préstamos.");
            Person user = model.AddPerson("User", "Solicita préstamos y consulta libros.");

            librarian.Uses(librarySystem, "Gestiona inventario y préstamos.");
            user.Uses(librarySystem, "Consulta catálogo y solicita préstamos.");
            librarySystem.Uses(notificationService, "Envía notificaciones push.");
            user.Uses(authenticationService, "Se registra y autentica.");
            librarySystem.Uses(authenticationService, "Valida usuarios.");

            SystemContextView contextView = viewSet.CreateSystemContextView(librarySystem, "Contexto", "Diagrama de Contexto del Sistema de Gestión de Biblioteca");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags y Estilos
            librarySystem.AddTags("LibrarySystem");
            notificationService.AddTags("NotificationService");
            authenticationService.AddTags("AuthenticationService");
            librarian.AddTags("Librarian");
            user.AddTags("User");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Librarian") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("User") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("LibrarySystem") { Background = "#1168bd", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("NotificationService") { Background = "#9370db", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("AuthenticationService") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });

            // Diagrama de Contenedores
            Container webApp = librarySystem.AddContainer("Web Application", "Interfaz para bibliotecarios.", "React");
            Container mobileApp = librarySystem.AddContainer("Mobile Application", "Interfaz para usuarios.", "Flutter");
            Container api = librarySystem.AddContainer("Library API", "Expone servicios para gestión de libros y préstamos.", "ASP.NET Core");
            Container database = librarySystem.AddContainer("Library Database", "Almacena datos de libros, usuarios y préstamos.", "SQL Server");
            Container notificationQueue = notificationService.AddContainer("Notification Queue", "Cola para manejar notificaciones.", "RabbitMQ");

            librarian.Uses(webApp, "Administra libros y préstamos.");
            user.Uses(mobileApp, "Consulta catálogo y solicita préstamos.");
            webApp.Uses(api, "Consulta y actualiza datos.");
            mobileApp.Uses(api, "Consulta y actualiza datos.");
            api.Uses(database, "Lee y escribe datos.");
            api.Uses(notificationQueue, "Publica eventos de notificación.");

            ContainerView containerView = viewSet.CreateContainerView(librarySystem, "Contenedores", "Diagrama de Contenedores del Sistema");
            containerView.AddAllElements();

            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}