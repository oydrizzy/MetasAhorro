## 💰 MetasAhorro

Aplicación web en **ASP.NET Core Razor Pages** para gestionar y visualizar metas de ahorro de forma clara, motivadora y organizada.

## 🚀 Características

- 📌 **Gestión de metas**: crear, ver, depositar y (opcional) eliminar metas.
- 📊 **Progreso visual**: gráficas de progreso individuales y globales con **Chart.js**.
- 🎯 **Resumen global**: muestra el objetivo total, lo ahorrado y lo faltante.
- 💬 **Frases motivadoras dinámicas** según tu nivel de progreso.
- 🎨 **Diseño moderno y responsive** usando **TailwindCSS + Bootstrap Icons**.

## 🖼️ Capturas
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000627" src="https://github.com/user-attachments/assets/5092dd94-6bf7-41a2-876a-bce7d508f6bc" />
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000641" src="https://github.com/user-attachments/assets/7af5e6dc-40e7-49ae-bf36-f3d8dfd30ebc" />
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000656" src="https://github.com/user-attachments/assets/c21e0033-5c59-4a4c-a996-2d3f9a2b9a25" />
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000735" src="https://github.com/user-attachments/assets/090eb212-77c5-416e-99cf-d4847805a6c2" />
<img width="1915" height="1079" alt="Captura de pantalla 2025-08-17 000748" src="https://github.com/user-attachments/assets/19aebe81-5b50-4c87-be24-17554f2ee700" />
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000758" src="https://github.com/user-attachments/assets/0914b1a7-9122-4636-a377-f5314f538d37" />
<img width="1919" height="1076" alt="Captura de pantalla 2025-08-17 000810" src="https://github.com/user-attachments/assets/c121a45d-f993-4567-8c37-17e2cbd96602" />
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000831" src="https://github.com/user-attachments/assets/376c6beb-ea30-4dfb-b8c3-170963c1c34d" />
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000910" src="https://github.com/user-attachments/assets/0324500d-f43f-474a-a0ca-8372bd1eac25" />
<img width="1919" height="1079" alt="Captura de pantalla 2025-08-17 000925" src="https://github.com/user-attachments/assets/fabd8b77-72f8-40bc-8383-1c17c47cab67" />



## ⚙️ Tecnologías utilizadas

- [ASP.NET Core 8 Razor Pages](https://learn.microsoft.com/aspnet/core/razor-pages)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQL Server LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Chart.js](https://www.chartjs.org/)
- [TailwindCSS](https://tailwindcss.com/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)

## 📂 Estructura principal

MetasAhorro/
│── Data/ # DbContext y configuración de la base de datos
│── Models/ # Modelos (Goal.cs)
│── Pages/Goals/ # Razor Pages para CRUD de metas
│ ├── Index.cshtml # Listado + resumen global
│ ├── New.cshtml # Crear nueva meta
│ ├── Deposit.cshtml # Depositar en una meta
│ ├── View.cshtml # Detalle de una meta
│── wwwroot/ # CSS, JS, imágenes y estáticos
│── appsettings.json # Configuración de la app


## 🛠️ Instalación y ejecución

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/oydrizzy/MetasAhorro.git
   cd MetasAhorro

2. Configurar la base de datos en appsettings.json:
   
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MetasAhorroDb;Trusted_Connection=True;"
}

3. Ejecutar migraciones:
   
dotnet ef database update

4. Levantar el proyecto:

dotnet run





