create database DB_Carrito

use DB_Carrito

go

create table Categoria(
IdCategoria int primary key identity(1,1),
Descripcion varchar (100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)

go

create table Marca(
IdMarca int primary key identity(1,1),
Descripcion varchar (100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)

go

create table Producto(
IdProducto int primary key identity(1,1),
Nombre varchar(100),
Descripcion varchar(500),
CategoriaId int references Categoria(IdCategoria),
MarcaId int references Marca(IdMarca),
Precio decimal(10,2) default 0,
Stock int,
RutaImagen varchar(100),
NombreImagen varchar (100),
Activo bit default 1,
FechaRegistro datetime default getdate()

)

go

create table Cliente(
IdCliente int primary key identity(1,1),
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Clave varchar(150),
Restablecer bit default 0,
FechaRegistro datetime default getdate()
) 

go

create table Carrito(
IdCarrito int primary key identity(1,1),
ClienteId int references Cliente(IdCliente),
ProductoId int references Producto(IdProducto),
Cantidad int
)

go

create table Venta(
IdVenta int primary key identity(1,1),
ClienteId int references Cliente(IdCliente),
TotapProducto int,
MotoTotal decimal(10,2),
Contacto varchar(50),
IdDistrito varchar(10),
Direccion varchar(100),
IdTransaccion varchar(50),
FechaVenta datetime default getdate()
)

go

create table DetalleVenta(
IdDetalleVenta int primary key identity(1,1),
VentaId int references Venta(IdVenta),
ProductoId int references Producto(IdProducto),
Cantidad int,
Total decimal(10,2)
)

go

create table Usuario(
IdUsuario int primary key identity(1,1),
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Clave varchar(150),
Restablecer bit default 1,
Activo bit default 1,
FechaRegistro datetime default getdate()
)

go

create table Departamento(
IdDepartamento varchar(2) not null,
Descripcion varchar(100)not null
)

go

create table Provincia(
IdProvincia varchar(4) not null,
Descripcion varchar(100) not null
)

go

create table Distrito(
IdDistrito varchar(6) not null,
Descripcion varchar(100) not null,
IdDepartamento varchar(2) not null,
IdProvincia varchar(4) not null
)

