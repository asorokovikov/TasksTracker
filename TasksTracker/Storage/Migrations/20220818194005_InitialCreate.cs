using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksTracker.Storage.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskId", "CreatedAt", "Name", "State" },
                values: new object[,]
                {
                    { new Guid("04f5c36f-6510-47d6-81e6-8cdba2b7c14e"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5304), "Task #39", 0 },
                    { new Guid("0652e5bf-b798-47d7-a612-61e41a6224e5"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5307), "Task #41", 0 },
                    { new Guid("081d93ba-237d-460b-a965-355097b60513"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5344), "Task #60", 0 },
                    { new Guid("08ba1b45-6a93-472f-9e0c-7d768fa17e6b"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5365), "Task #70", 0 },
                    { new Guid("0abc8693-5d70-48fd-807b-39a0cb39f7d3"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5330), "Task #53", 0 },
                    { new Guid("0c4fdab0-0622-494f-a598-4738b0543e08"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5424), "Task #91", 0 },
                    { new Guid("0f88a9f0-ddd1-47ea-8fc0-5f7e4ef26003"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5337), "Task #56", 0 },
                    { new Guid("0feea68f-66f4-42ae-b128-b532b849ec9c"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5349), "Task #62", 0 },
                    { new Guid("11572be4-842d-4908-876f-63d4e7d60005"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5298), "Task #37", 0 },
                    { new Guid("17a621e7-335c-4dee-a4e4-8946314ae29a"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5417), "Task #87", 0 },
                    { new Guid("1cb28e2e-79de-4e87-9b6c-7489b824cb25"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5317), "Task #46", 0 },
                    { new Guid("1f302255-db2b-41c1-8adb-c7fcf4c65025"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5231), "Task #14", 0 },
                    { new Guid("22d719fe-69b2-4c5f-bfb4-ec2ed3fa915c"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5249), "Task #23", 0 },
                    { new Guid("23ba9de0-55cd-4581-833a-5fb724a990a5"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5328), "Task #52", 0 },
                    { new Guid("24461e16-acb0-47e8-b099-21e4776e709b"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5419), "Task #88", 0 },
                    { new Guid("2a3f6db0-0359-4bee-8457-dd8ae92650b3"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5232), "Task #15", 0 },
                    { new Guid("2a9a5784-37a1-40f3-9c68-fe9bb1157aa5"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5400), "Task #78", 0 },
                    { new Guid("2c38b672-7fa3-4f99-ac4a-e39d58570b92"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5202), "Task #5", 0 },
                    { new Guid("2cfae9cd-c43b-46d2-958f-13dca70d5063"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5436), "Task #97", 0 },
                    { new Guid("2daffb60-a5cb-4ab7-9b1b-24bf6c28149d"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5333), "Task #54", 0 },
                    { new Guid("2e1d2ce2-86b5-41a6-abc1-c3317b0d4858"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5359), "Task #67", 0 },
                    { new Guid("2fdd90aa-06e7-4c6f-ae30-c324bae697d5"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5410), "Task #84", 0 },
                    { new Guid("32ca1da3-c6af-469d-913d-a3497f47618f"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5426), "Task #92", 0 },
                    { new Guid("36175191-4525-422e-b7e4-01d9c44bc3af"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5441), "Task #100", 0 },
                    { new Guid("371e48ee-f39d-463f-9b86-0c68fec3c3b1"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5254), "Task #26", 0 },
                    { new Guid("3b60f965-0272-49c6-81ae-c65b4f1cbb4b"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5255), "Task #27", 0 },
                    { new Guid("3b67362b-9b8e-4f27-b2cd-8ab83aa5d62f"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5244), "Task #21", 0 },
                    { new Guid("3bfa0b04-bcb1-40b1-868a-859713e4caf5"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5367), "Task #71", 0 },
                    { new Guid("403b54ec-bd5a-4c6d-ab17-0ed2ca40264d"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5376), "Task #76", 0 },
                    { new Guid("42435191-efb9-46af-b6e4-f8d520b52bce"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5250), "Task #24", 0 },
                    { new Guid("43e6aba3-c1c3-4b0e-878c-f985d77cab33"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5370), "Task #73", 0 },
                    { new Guid("447b9a06-539b-43ae-9a84-ff4725096158"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5321), "Task #48", 0 },
                    { new Guid("481d253c-7151-41f9-8cbc-8215e9989335"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5350), "Task #63", 0 },
                    { new Guid("49d16065-b47f-4cc2-bdba-34d100823113"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5434), "Task #96", 0 },
                    { new Guid("4e71e6c8-56a0-4e75-9706-236d070975b3"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5236), "Task #17", 0 },
                    { new Guid("50ced3ef-84de-4ddc-803d-67fb8f6e85c1"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5438), "Task #98", 0 },
                    { new Guid("5567fb1e-ad35-48bb-8a44-699c8f023773"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5407), "Task #82", 0 },
                    { new Guid("57960fe7-1b4a-4766-aa43-9f1fad452334"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5354), "Task #65", 0 },
                    { new Guid("5c3df405-a675-4737-803f-3c3ba197d7e7"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5196), "Task #2", 0 },
                    { new Guid("5ddd29eb-3828-4c6d-922b-204ce53b1311"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5374), "Task #75", 0 },
                    { new Guid("5fafaacd-aed1-4b7e-8337-803b9127cbcf"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5323), "Task #49", 0 },
                    { new Guid("60a264df-eacd-4752-a5c5-9eac69c54d0c"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5346), "Task #61", 0 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskId", "CreatedAt", "Name", "State" },
                values: new object[,]
                {
                    { new Guid("61cf5c43-cd6f-4773-8187-5f85b5990f32"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5357), "Task #66", 0 },
                    { new Guid("62404a9f-c786-48b0-8349-eb799f035f8a"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5352), "Task #64", 0 },
                    { new Guid("6647f234-45f2-495b-8445-e77f586e706b"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5264), "Task #31", 0 },
                    { new Guid("6b8a7c4e-e939-451d-8dc3-59270573bb0f"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5360), "Task #68", 0 },
                    { new Guid("6c34db21-9ab0-465e-a1e5-4abae9d46553"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5420), "Task #89", 0 },
                    { new Guid("6f3b07fd-272d-4dda-8d22-3120c00cb907"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5234), "Task #16", 0 },
                    { new Guid("705b621e-dfd9-4aa7-9edf-5bc111f7d903"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5405), "Task #81", 0 },
                    { new Guid("70cc0554-b28a-443a-aeb1-b0f1613bbea3"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5369), "Task #72", 0 },
                    { new Guid("7587336f-40c7-45a5-925a-ba94fafde703"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5262), "Task #30", 0 },
                    { new Guid("7f3c4489-852b-42a3-aa4b-3e4400103384"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5238), "Task #18", 0 },
                    { new Guid("80dfbb0b-2636-447a-b4ed-0214451eef7e"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5266), "Task #32", 0 },
                    { new Guid("8204936f-7af6-4cc7-b5d5-c3fe895aa6b7"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5200), "Task #4", 0 },
                    { new Guid("82528760-f445-49a7-aa57-963941ac7f62"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5403), "Task #80", 0 },
                    { new Guid("849980fd-e593-4cec-9d94-c443fbcbc1fc"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5295), "Task #35", 0 },
                    { new Guid("8582daae-6735-4fc5-a64a-5286466160d4"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5335), "Task #55", 0 },
                    { new Guid("8653f92a-4a92-48e0-9292-04ff9a821e4d"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5342), "Task #59", 0 },
                    { new Guid("8e2a4354-0798-4511-8ddf-73b22dfe8fdd"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5311), "Task #43", 0 },
                    { new Guid("8ec37d81-e250-4b8c-b2cf-48997bb165c4"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5397), "Task #77", 0 },
                    { new Guid("9e51bde2-7a37-4968-a0fd-8c6616ef6f20"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5305), "Task #40", 0 },
                    { new Guid("9e869010-1105-4901-86ec-3ce9b0aa5dcb"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5412), "Task #85", 0 },
                    { new Guid("a8d10736-52a4-4135-87a2-9bc1ee4160ab"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5431), "Task #94", 0 },
                    { new Guid("a91af55d-d68c-444f-bef0-a83f8b2a3540"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5252), "Task #25", 0 },
                    { new Guid("a96514f6-2b68-476a-9d36-03272bfe1437"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5409), "Task #83", 0 },
                    { new Guid("acf36704-ee54-4b95-907a-666dc281fcde"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5242), "Task #20", 0 },
                    { new Guid("ae69b863-ca8d-4e90-9a87-a6226aea5cb9"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5217), "Task #8", 0 },
                    { new Guid("b4b1f61c-ed07-4a20-bd40-ce6beda92fe4"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5227), "Task #13", 0 },
                    { new Guid("b62dcab0-1aed-43fa-bac0-bbea714c9ef6"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5223), "Task #11", 0 },
                    { new Guid("b6ff5fb3-1176-45cd-8187-48939bcbf794"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5257), "Task #28", 0 },
                    { new Guid("b75406b3-c505-4ce7-b710-09795f8c9e37"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5219), "Task #9", 0 },
                    { new Guid("b7d939fd-468e-44cf-bf44-4b47e53d18b8"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5415), "Task #86", 0 },
                    { new Guid("ba846ff2-5eb1-42be-9020-c2e0c959a0cd"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5439), "Task #99", 0 },
                    { new Guid("ba9563b5-37a3-41f4-9f38-3156f7e22fbf"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5302), "Task #38", 0 },
                    { new Guid("bdee83d1-ad59-4d78-826f-616021117305"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5402), "Task #79", 0 },
                    { new Guid("c3757032-d6d3-4a6c-ac42-9ca11c4b320a"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5182), "Task #1", 0 },
                    { new Guid("c44ef5eb-8bf4-4ac3-b3d9-e5961c18f2ae"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5225), "Task #12", 0 },
                    { new Guid("c587d65c-f785-4916-a470-138a3ea65e9c"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5432), "Task #95", 0 },
                    { new Guid("c657f8d4-289a-47fc-a9cf-c436cccb9dc9"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5247), "Task #22", 0 },
                    { new Guid("cd705024-f56e-4f99-b2a0-617842c70e30"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5339), "Task #57", 0 },
                    { new Guid("cd8a3d89-815b-4c18-a2ac-511801b216ef"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5340), "Task #58", 0 },
                    { new Guid("ce30e8ff-e9cf-453f-ab74-3227aa9a5d0a"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5314), "Task #45", 0 },
                    { new Guid("d2933fc2-7a72-49d8-a421-c638f6549efb"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5309), "Task #42", 0 },
                    { new Guid("d6397912-ab1d-474f-b1d2-2650407e61bc"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5326), "Task #51", 0 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskId", "CreatedAt", "Name", "State" },
                values: new object[,]
                {
                    { new Guid("d66282a2-4aab-4881-82f7-656354d815fd"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5270), "Task #34", 0 },
                    { new Guid("dcacd144-f54e-46e3-8cb5-aa74d47dc853"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5268), "Task #33", 0 },
                    { new Guid("e4a9627d-8e8f-4f06-b977-9ab709414c7e"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5362), "Task #69", 0 },
                    { new Guid("e95cbdfa-1c4b-416d-902e-fc809049de93"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5297), "Task #36", 0 },
                    { new Guid("ed1ef4e3-b1b6-4aa4-a6f1-9a4974259288"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5319), "Task #47", 0 },
                    { new Guid("ef1820b5-f06d-4a6b-9e19-7834c898c668"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5324), "Task #50", 0 },
                    { new Guid("eff057ce-122a-45ff-b617-bf4d5562aed6"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5198), "Task #3", 0 },
                    { new Guid("f0e4d276-e8ac-49a5-b8fa-a18b6c97ac08"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5215), "Task #7", 0 },
                    { new Guid("f172fc25-7bc8-4d2b-a2b0-2ae1ac584dcd"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5259), "Task #29", 0 },
                    { new Guid("f2518efb-6640-4d63-9773-88841f833e77"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5372), "Task #74", 0 },
                    { new Guid("f375b603-3fcb-4755-bad1-170800269196"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5427), "Task #93", 0 },
                    { new Guid("f3f8de08-55a5-40b0-8b36-54292695d73d"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5213), "Task #6", 0 },
                    { new Guid("f5c42aa2-4b91-4083-bf31-c669e5f959d0"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5312), "Task #44", 0 },
                    { new Guid("f9bc0dac-2aa6-4340-b338-d73d9e415112"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5221), "Task #10", 0 },
                    { new Guid("fa538093-d3e1-4699-a1b6-a829d268be9e"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5240), "Task #19", 0 },
                    { new Guid("fe1252cf-901c-424f-8728-490b9e7685af"), new DateTime(2022, 8, 18, 22, 40, 5, 795, DateTimeKind.Local).AddTicks(5422), "Task #90", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TaskId",
                table: "Attachments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedAt",
                table: "Tasks",
                column: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
