using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LorafyAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EndDevices",
                columns: table => new
                {
                    EUI = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndDevices", x => x.EUI);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Gateways",
                columns: table => new
                {
                    EUI = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RSSI = table.Column<int>(type: "int", nullable: false),
                    SNR = table.Column<float>(type: "float", nullable: false),
                    LocationLatitude = table.Column<float>(name: "Location_Latitude", type: "float", nullable: true),
                    LocationLongitude = table.Column<float>(name: "Location_Longitude", type: "float", nullable: true),
                    LocationAltitude = table.Column<float>(name: "Location_Altitude", type: "float", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gateways", x => x.EUI);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UplinkMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EndDeviceEUI = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GatewayEUI = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayloadBattery = table.Column<float>(name: "Payload_Battery", type: "float", nullable: true),
                    PayloadBatteryVoltage = table.Column<float>(name: "Payload_BatteryVoltage", type: "float", nullable: true),
                    PayloadTemperatureInside = table.Column<float>(name: "Payload_TemperatureInside", type: "float", nullable: true),
                    PayloadTemperatureOutside = table.Column<float>(name: "Payload_TemperatureOutside", type: "float", nullable: true),
                    PayloadHumidity = table.Column<float>(name: "Payload_Humidity", type: "float", nullable: true),
                    PayloadLight = table.Column<float>(name: "Payload_Light", type: "float", nullable: true),
                    PayloadPressure = table.Column<float>(name: "Payload_Pressure", type: "float", nullable: true),
                    DataRateBandwidth = table.Column<int>(name: "DataRate_Bandwidth", type: "int", nullable: false),
                    DataRateSpreadingFactor = table.Column<int>(name: "DataRate_SpreadingFactor", type: "int", nullable: false),
                    DataRateCodingRate = table.Column<string>(name: "DataRate_CodingRate", type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateReceived = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UplinkMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UplinkMessages_EndDevices_EndDeviceEUI",
                        column: x => x.EndDeviceEUI,
                        principalTable: "EndDevices",
                        principalColumn: "EUI",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UplinkMessages_Gateways_GatewayEUI",
                        column: x => x.GatewayEUI,
                        principalTable: "Gateways",
                        principalColumn: "EUI",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UplinkMessages_EndDeviceEUI",
                table: "UplinkMessages",
                column: "EndDeviceEUI");

            migrationBuilder.CreateIndex(
                name: "IX_UplinkMessages_GatewayEUI",
                table: "UplinkMessages",
                column: "GatewayEUI");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UplinkMessages");

            migrationBuilder.DropTable(
                name: "EndDevices");

            migrationBuilder.DropTable(
                name: "Gateways");
        }
    }
}
