using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matissa.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    idCliente = table.Column<int>(type: "int", nullable: false),
                    nombreCliente = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidoCliente = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    teléfono = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    nacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    dirección = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cliente__885457EE5A51A261", x => x.idCliente);
                });

            migrationBuilder.CreateTable(
                name: "compra",
                columns: table => new
                {
                    idCompra = table.Column<int>(type: "int", nullable: false),
                    descripción = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fechaCompra = table.Column<DateTime>(type: "date", nullable: false),
                    costoTotalCompra = table.Column<double>(type: "float", nullable: false),
                    factura = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__compra__48B99DB7A13325F8", x => x.idCompra);
                });

            migrationBuilder.CreateTable(
                name: "empleado",
                columns: table => new
                {
                    idEmpleado = table.Column<int>(type: "int", nullable: false),
                    nombreEmpleado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidoEmpleado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    genero = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    fechaContrato = table.Column<DateTime>(type: "date", nullable: false),
                    fechaNacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    dirección = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    teléfono = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__empleado__5295297C9779403D", x => x.idEmpleado);
                });

            migrationBuilder.CreateTable(
                name: "permiso",
                columns: table => new
                {
                    idPermiso = table.Column<int>(type: "int", nullable: false),
                    modulo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripción = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__permiso__06A58486FA2DAA24", x => x.idPermiso);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    nombreProducto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripción = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fechaCaducidad = table.Column<DateTime>(type: "date", nullable: false),
                    precioVenta = table.Column<double>(type: "float", nullable: false),
                    saldoInventario = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producto__07F4A132E6BA3F28", x => x.idProducto);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    idRol = table.Column<int>(type: "int", nullable: false),
                    nombreRol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__rol__3C872F7600876082", x => x.idRol);
                });

            migrationBuilder.CreateTable(
                name: "tipoServicio",
                columns: table => new
                {
                    idTipoServicio = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tipoServ__278616767C8DCAAE", x => x.idTipoServicio);
                });

            migrationBuilder.CreateTable(
                name: "cita",
                columns: table => new
                {
                    idCita = table.Column<int>(type: "int", nullable: false),
                    fechaRegistro = table.Column<DateTime>(type: "date", nullable: false),
                    costoTotal = table.Column<double>(type: "float", nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false),
                    idCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cita__814F31263DA04DE3", x => x.idCita);
                    table.ForeignKey(
                        name: "FK__cita__idCliente__4F7CD00D",
                        column: x => x.idCliente,
                        principalTable: "cliente",
                        principalColumn: "idCliente");
                });

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    idPedido = table.Column<int>(type: "int", nullable: false),
                    idCliente = table.Column<int>(type: "int", nullable: false),
                    fechaPedido = table.Column<DateTime>(type: "date", nullable: false),
                    precioTotalPedido = table.Column<double>(type: "float", nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pedido__A9F619B752D55725", x => x.idPedido);
                    table.ForeignKey(
                        name: "FK__pedido__idClient__5EBF139D",
                        column: x => x.idCliente,
                        principalTable: "cliente",
                        principalColumn: "idCliente");
                });

            migrationBuilder.CreateTable(
                name: "rolXPermiso",
                columns: table => new
                {
                    idRolXPermiso = table.Column<int>(type: "int", nullable: false),
                    idRol = table.Column<int>(type: "int", nullable: false),
                    idPermiso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__rolXPerm__3DC6779B486DF950", x => x.idRolXPermiso);
                    table.ForeignKey(
                        name: "FK__rolXPermi__idPer__3F466844",
                        column: x => x.idPermiso,
                        principalTable: "permiso",
                        principalColumn: "idPermiso");
                    table.ForeignKey(
                        name: "FK__rolXPermi__idRol__3E52440B",
                        column: x => x.idRol,
                        principalTable: "rol",
                        principalColumn: "idRol");
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    nombreUsuario = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    contraseña = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false),
                    idRol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuario__645723A6099093C0", x => x.idUsuario);
                    table.ForeignKey(
                        name: "FK__usuario__idRol__3B75D760",
                        column: x => x.idRol,
                        principalTable: "rol",
                        principalColumn: "idRol");
                });

            migrationBuilder.CreateTable(
                name: "servicio",
                columns: table => new
                {
                    idServicio = table.Column<int>(type: "int", nullable: false),
                    nombreServicio = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripción = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    duración = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<double>(type: "float", nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false),
                    idTipoServicio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__servicio__CEB98119B2088B0F", x => x.idServicio);
                    table.ForeignKey(
                        name: "FK__servicio__idTipo__4CA06362",
                        column: x => x.idTipoServicio,
                        principalTable: "tipoServicio",
                        principalColumn: "idTipoServicio");
                });

            migrationBuilder.CreateTable(
                name: "ventaServicio",
                columns: table => new
                {
                    idVentaServicio = table.Column<int>(type: "int", nullable: false),
                    idCita = table.Column<int>(type: "int", nullable: false),
                    valorVenta = table.Column<double>(type: "float", nullable: false),
                    fechaVenta = table.Column<DateTime>(type: "date", nullable: false),
                    formaPago = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    estado = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ventaSer__59B03D9C75C79989", x => x.idVentaServicio);
                    table.ForeignKey(
                        name: "FK__ventaServ__idCit__68487DD7",
                        column: x => x.idCita,
                        principalTable: "cita",
                        principalColumn: "idCita");
                });

            migrationBuilder.CreateTable(
                name: "detallePedido",
                columns: table => new
                {
                    idDetallePedido = table.Column<int>(type: "int", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    idPedido = table.Column<int>(type: "int", nullable: false),
                    cantidadProducto = table.Column<int>(type: "int", nullable: false),
                    precioUnitario = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detalleP__610F0056E942CCDA", x => x.idDetallePedido);
                    table.ForeignKey(
                        name: "FK__detallePe__idPed__628FA481",
                        column: x => x.idPedido,
                        principalTable: "pedido",
                        principalColumn: "idPedido");
                    table.ForeignKey(
                        name: "FK__detallePe__idPro__619B8048",
                        column: x => x.idProducto,
                        principalTable: "producto",
                        principalColumn: "idProducto");
                });

            migrationBuilder.CreateTable(
                name: "detalleCita",
                columns: table => new
                {
                    idDetalleCita = table.Column<int>(type: "int", nullable: false),
                    idCita = table.Column<int>(type: "int", nullable: false),
                    idEmpleado = table.Column<int>(type: "int", nullable: false),
                    idServicio = table.Column<int>(type: "int", nullable: false),
                    fechaCita = table.Column<DateTime>(type: "date", nullable: false),
                    horaInicio = table.Column<int>(type: "int", nullable: false),
                    horaFin = table.Column<int>(type: "int", nullable: false),
                    duraciónServicio = table.Column<int>(type: "int", nullable: false),
                    descuento = table.Column<double>(type: "float", nullable: false),
                    costoServicio = table.Column<double>(type: "float", nullable: false),
                    estado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detalleC__031271117A36A6BB", x => x.idDetalleCita);
                    table.ForeignKey(
                        name: "FK__detalleCi__idCit__52593CB8",
                        column: x => x.idCita,
                        principalTable: "cita",
                        principalColumn: "idCita");
                    table.ForeignKey(
                        name: "FK__detalleCi__idEmp__534D60F1",
                        column: x => x.idEmpleado,
                        principalTable: "empleado",
                        principalColumn: "idEmpleado");
                    table.ForeignKey(
                        name: "FK__detalleCi__idSer__5441852A",
                        column: x => x.idServicio,
                        principalTable: "servicio",
                        principalColumn: "idServicio");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cita_idCliente",
                table: "cita",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_detalleCita_idCita",
                table: "detalleCita",
                column: "idCita");

            migrationBuilder.CreateIndex(
                name: "IX_detalleCita_idEmpleado",
                table: "detalleCita",
                column: "idEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_detalleCita_idServicio",
                table: "detalleCita",
                column: "idServicio");

            migrationBuilder.CreateIndex(
                name: "IX_detallePedido_idPedido",
                table: "detallePedido",
                column: "idPedido");

            migrationBuilder.CreateIndex(
                name: "IX_detallePedido_idProducto",
                table: "detallePedido",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_idCliente",
                table: "pedido",
                column: "idCliente");

            migrationBuilder.CreateIndex(
                name: "IX_rolXPermiso_idPermiso",
                table: "rolXPermiso",
                column: "idPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_rolXPermiso_idRol",
                table: "rolXPermiso",
                column: "idRol");

            migrationBuilder.CreateIndex(
                name: "IX_servicio_idTipoServicio",
                table: "servicio",
                column: "idTipoServicio");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_idRol",
                table: "usuario",
                column: "idRol");

            migrationBuilder.CreateIndex(
                name: "IX_ventaServicio_idCita",
                table: "ventaServicio",
                column: "idCita");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "compra");

            migrationBuilder.DropTable(
                name: "detalleCita");

            migrationBuilder.DropTable(
                name: "detallePedido");

            migrationBuilder.DropTable(
                name: "rolXPermiso");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "ventaServicio");

            migrationBuilder.DropTable(
                name: "empleado");

            migrationBuilder.DropTable(
                name: "servicio");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "permiso");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "cita");

            migrationBuilder.DropTable(
                name: "tipoServicio");

            migrationBuilder.DropTable(
                name: "cliente");
        }
    }
}
