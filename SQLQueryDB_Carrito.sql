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

////////////////////////////////////////////////////////
create proc sp_AgregarUsuario
@Nombres varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@Activo bit, 
@Mensaje varchar(100) output,
@Resultado int output
as
begin
	set @Resultado= 0

	if not exists(select * from Usuario where Correo= @Correo )

	begin
		insert into Usuario (Nombres, Apellidos, Correo,Clave, Activo) 
		values (@Nombres, @Apellidos, @Correo,@Clave, @Activo)

		set @Resultado= Scope_Identity()
	end
	else
		set @Mensaje= 'El correo ingresado ya existe'
end
//////////////////////////////////////////////////////////////////////////
go
create procedure sp_EditarUsuario
@IdUsuario int,
@Nombres varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Activo bit, 
@Mensaje varchar(100) output,
@Resultado bit output
as

begin
	set @Resultado= 0

	if not exists(select * from Usuario where Correo=@Correo and IdUsuario != @IdUsuario)
	begin
		update top(1) Usuario set Nombres=@Nombres, Apellidos=@Apellidos, Correo=@Correo,
		Activo=@Activo where IdUsuario= @IdUsuario
		
		set @Resultado= 1
	end

	else
		set @Mensaje= 'El Correo del usuario ya existe'
end
//////////////////////////////////////////////////////////////
go

create proc sp_AgregarCategoria
@Descripcion varchar (100),
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
as
begin
	set @Resultado= 0

	if not exists(select * from Categoria where Descripcion= @Descripcion )

	begin
		insert into Categoria(Descripcion, Activo) 
		values (@Descripcion, @Activo)

		set @Resultado= Scope_Identity()
	end
	else
		set @Mensaje= 'La Categoria ingresada ya existe'

end
////////////////////////////////////////////////////////////////////
go

create procedure sp_EditarCategoria
@IdCategoria int,
@Descripcion varchar(100),
@Activo bit ,
@Mensaje varchar(100) output,
@Resultado bit output
as

begin
	set @Resultado= 0

	if not exists(select * from Categoria where Descripcion=@Descripcion and IdCategoria != @IdCategoria)
	begin
		update top(1) Categoria set Descripcion=@Descripcion, Activo=@Activo
		where IdCategoria= @IdCategoria
		
		set @Resultado= 1
	end

	else
		set @Mensaje= 'La Categoria ya existe'
end
///////////////////////////////////////////////////////////
go
create proc sp_EliminarCategoria
@IdCategoria int,
@Mensaje varchar(100) output,
@Resultado bit output

as
begin
	set @Resultado=0
	if not exists(select * from Producto p
	inner join Categoria c on c.IdCategoria=p.CategoriaId
	where p.CategoriaId=@IdCategoria )
	begin
		delete top(1) from Categoria where IdCategoria=@IdCategoria
		set @Resultado=1
	end
	else
	set @Mensaje='La Categoria se encuentra relaconada a un prodructo'

end
//////////////////////////////////////////////////////////////////////////////
go
create proc sp_AgregarMarca
@Descripcion varchar (500),
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
as
begin
	set @Resultado= 0

	if not exists(select * from Marca where Descripcion= @Descripcion )

	begin
		insert into Marca(Descripcion, Activo) 
		values (@Descripcion, @Activo)

		set @Resultado= Scope_Identity()
	end
	else
		set @Mensaje= 'La Marca ingresada ya existe'

end
//////////////////////////////////////////////////////////////////////
go
create procedure sp_EditarMarca
@IdMarca int,
@Descripcion varchar(500),
@Activo bit ,
@Mensaje varchar(100) output,
@Resultado bit output
as

begin
	set @Resultado= 0

	if not exists(select * from Marca where Descripcion=@Descripcion and IdMarca != @IdMarca)
	begin
		update top(1) Marca set Descripcion=@Descripcion, Activo=@Activo
		where IdMarca= @IdMarca
		
		set @Resultado= 1
	end

	else
		set @Mensaje= 'La Marca ya existe'
end
////////////////////////////////////////////////////////////////////////
go
create proc sp_EliminarMarca
@IdMarca int,
@Mensaje varchar(100) output,
@Resultado bit output

as
begin
	set @Resultado=0
	if not exists(select * from Producto p
	inner join Marca m on m.IdMarca =p.MarcaId
	where p.MarcaId=@IdMarca )
	begin
		delete top(1) from Marca where IdMarca=@IdMarca
		set @Resultado=1
	end
	else
	set @Mensaje='La Marca se encuentra relaconada a un prodructo'

end
//////////////////////////////////////////////////////////////////

go
create proc sp_AgregarProducto
@Nombre varchar (100),
@Descripcion varchar (500),
@CategoriaId int,
@MarcaId int,
@Precio decimal(10,2),
@Stock int,
@Activo bit,
@Mensaje varchar(500) output,
@Resultado int output
as
begin
	set @Resultado= 0

	if not exists(select * from Producto where Nombre= @Nombre )

	begin
		insert into Producto(Nombre,Descripcion,CategoriaId,MarcaId,Precio,Stock,Activo) 
		values (@Nombre,@Descripcion,@CategoriaId,@MarcaId,@Precio,@Stock,@Activo)

		set @Resultado= Scope_Identity()
	end
	else
		set @Mensaje= 'El Producto ingresado ya existe'
end
//////////////////////////////////////////////////////////////////////
go
create procedure sp_EditarProducto
@IdProducto int,
@Nombre varchar (100),
@Descripcion varchar (500),
@CategoriaId int,
@MarcaId int,
@Precio decimal(10,2),
@Stock int,
@Activo bit ,
@Mensaje varchar(100) output,
@Resultado bit output
as

begin
	set @Resultado= 0

	if not exists(select * from Producto where Nombre=Nombre and IdProducto != @IdProducto)
	begin
		update top(1) Producto set Nombre=@Nombre,Descripcion=@Descripcion,CategoriaId=@CategoriaId,
					 MarcaId=@MarcaId,Precio=@Precio,Stock=@Stock,Activo=@Activo
		where IdProducto= @IdProducto
		
		set @Resultado= 1
	end

	else
		set @Mensaje= 'El Producto ya existe'
end
///////////////////////////////////////////////////////////////////////////
go
create proc sp_EliminarProducto
@IdProducto int,
@Mensaje varchar(100) output,
@Resultado bit output

as
begin
	set @Resultado=0
	if not exists(select * from DetalleVenta dv
	inner join Producto p on p.IdProducto = dv.ProductoId
	where p.IdProducto =@IdProducto )
	begin
		delete top(1) from Producto where IdProducto=@IdProducto
		set @Resultado=1
	end
	else
	set @Mensaje='El producto esta relacionado a una venta'

end
////////////////////////////////////////////////////////////////////////

select p.IdProducto,p.Nombre,p.Descripcion,
m.IdMarca,m.Descripcion[DesMarca],
c.IdCategoria,c.Descripcion[DesCategoria],
p.Precio,p.Stock,p.RutaImagen,p.NombreImagen,p.Activo
 from Producto p
inner join Marca m on m.IdMarca=p.MarcaId
inner join Categoria c on c.IdCategoria=p.CategoriaId
go
//////////////////////////////////////////////////////////////////////
go
create Proc sp_ReporteDashboard
as
begin

select

(select count(*) from Cliente) [TotalCliente],
(select isnull( sum(Cantidad),0) from DetalleVenta) [TotalVenta], 
(select count(*) from Producto) [TotalProducto]

end

exec sp_ReporteDashboard

/////////////////////////////////////////////////////////////////////////

create proc sp_ReporteVentas(
@FechaInicio varchar(10),
@FechaFin varchar(10),
@IdTransaccion varchar(50)
)
as
begin
		set dateformat dmy
		/*CONVERT( char(10), v.FechaVenta,103) muestra solo la fecha*/

		select CONVERT( char(10), v.FechaVenta,103)[FechaVenta],CONCAT( c.Nombres,' ',c.Apellidos)[Cliente],
		p.Nombre[Producto], p.Precio,dv.Cantidad,dv.Total, v.IdTransaccion
		from DetalleVenta dv
		inner join Producto p on p.IdProducto=dv.ProductoId
		inner join Venta v on v.IdVenta= dv.VentaId
		inner join Cliente c on c.IdCliente= v.ClienteId
		where CONVERT(date, v.FechaVenta) between @FechaInicio and @FechaFin 
		and v.IdTransaccion=iif(@IdTransaccion ='',v.IdTransaccion,@IdTransaccion)

end