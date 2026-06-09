IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260602194329_Inicial'
)
BEGIN
    CREATE TABLE [Estadios] (
        [Nombre] nvarchar(80) NOT NULL,
        [Capacidad] int NOT NULL,
        [Ciudad] nvarchar(60) NOT NULL,
        [Descripcion] nvarchar(400) NULL,
        CONSTRAINT [PK_Estadios] PRIMARY KEY ([Nombre])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260602194329_Inicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260602194329_Inicial', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260602213237_AgregarEquipo'
)
BEGIN
    CREATE TABLE [Equipos] (
        [Nombre] nvarchar(60) NOT NULL,
        [Confederacion] nvarchar(20) NOT NULL,
        [RankingFifa] int NOT NULL,
        CONSTRAINT [PK_Equipos] PRIMARY KEY ([Nombre])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260602213237_AgregarEquipo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260602213237_AgregarEquipo', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260602224623_UsuarioMigracion'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Apellido] nvarchar(100) NOT NULL,
        [Email] nvarchar(200) NOT NULL,
        [FechaNacimiento] datetime2 NOT NULL,
        [Contrasena] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260602224623_UsuarioMigracion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260602224623_UsuarioMigracion', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260603012904_LogAuditoriaMigracion'
)
BEGIN
    CREATE TABLE [LogsAuditoria] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] datetime2 NOT NULL,
        [Accion] nvarchar(500) NOT NULL,
        [UsuarioId] int NOT NULL,
        CONSTRAINT [PK_LogsAuditoria] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_LogsAuditoria_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260603012904_LogAuditoriaMigracion'
)
BEGIN
    CREATE INDEX [IX_LogsAuditoria_UsuarioId] ON [LogsAuditoria] ([UsuarioId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260603012904_LogAuditoriaMigracion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260603012904_LogAuditoriaMigracion', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260603200533_PersistirRolesUsuario'
)
BEGIN
    ALTER TABLE [Usuarios] ADD [Roles] nvarchar(100) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260603200533_PersistirRolesUsuario'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260603200533_PersistirRolesUsuario', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE TABLE [Grupos] (
        [Id] int NOT NULL IDENTITY,
        [Etiqueta] nvarchar(2) NOT NULL,
        CONSTRAINT [PK_Grupos] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE TABLE [Partidos] (
        [Id] int NOT NULL IDENTITY,
        [OrigenLocalId] int NULL,
        [OrigenVisitanteId] int NULL,
        [EstaBloqueado] bit NOT NULL,
        [TieneResultado] bit NOT NULL,
        [EsPorPerdedor] bit NOT NULL,
        [GolesLocalAnterior] int NOT NULL,
        [GolesVisitanteAnterior] int NOT NULL,
        [Codigo] nvarchar(20) NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [Fase] nvarchar(30) NOT NULL,
        [EquipoLocalNombre] nvarchar(60) NULL,
        [EquipoVisitanteNombre] nvarchar(60) NULL,
        [EstadioNombre] nvarchar(80) NULL,
        [GrupoId] int NULL,
        [GolesLocal] int NOT NULL,
        [GolesVisitante] int NOT NULL,
        [VencedorNombre] nvarchar(60) NULL,
        CONSTRAINT [PK_Partidos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Partidos_Equipos_EquipoLocalNombre] FOREIGN KEY ([EquipoLocalNombre]) REFERENCES [Equipos] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Partidos_Equipos_EquipoVisitanteNombre] FOREIGN KEY ([EquipoVisitanteNombre]) REFERENCES [Equipos] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Partidos_Equipos_VencedorNombre] FOREIGN KEY ([VencedorNombre]) REFERENCES [Equipos] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Partidos_Estadios_EstadioNombre] FOREIGN KEY ([EstadioNombre]) REFERENCES [Estadios] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Partidos_Grupos_GrupoId] FOREIGN KEY ([GrupoId]) REFERENCES [Grupos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Partidos_Partidos_OrigenLocalId] FOREIGN KEY ([OrigenLocalId]) REFERENCES [Partidos] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Partidos_Partidos_OrigenVisitanteId] FOREIGN KEY ([OrigenVisitanteId]) REFERENCES [Partidos] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE TABLE [PosicionesGrupo] (
        [Id] int NOT NULL IDENTITY,
        [DiferenciaGoles] int NOT NULL,
        [EquipoNombre] nvarchar(60) NOT NULL,
        [GrupoId] int NOT NULL,
        [Puntos] int NOT NULL,
        [GolesFavor] int NOT NULL,
        [GolesContra] int NOT NULL,
        [PosicionFinal] int NOT NULL,
        CONSTRAINT [PK_PosicionesGrupo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PosicionesGrupo_Equipos_EquipoNombre] FOREIGN KEY ([EquipoNombre]) REFERENCES [Equipos] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PosicionesGrupo_Grupos_GrupoId] FOREIGN KEY ([GrupoId]) REFERENCES [Grupos] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_Partidos_EquipoLocalNombre] ON [Partidos] ([EquipoLocalNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_Partidos_EquipoVisitanteNombre] ON [Partidos] ([EquipoVisitanteNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_Partidos_EstadioNombre] ON [Partidos] ([EstadioNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_Partidos_GrupoId] ON [Partidos] ([GrupoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_Partidos_OrigenLocalId] ON [Partidos] ([OrigenLocalId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_Partidos_OrigenVisitanteId] ON [Partidos] ([OrigenVisitanteId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_Partidos_VencedorNombre] ON [Partidos] ([VencedorNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_PosicionesGrupo_EquipoNombre] ON [PosicionesGrupo] ([EquipoNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    CREATE INDEX [IX_PosicionesGrupo_GrupoId] ON [PosicionesGrupo] ([GrupoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604121636_PersistirGrupoPosicionesPartido'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260604121636_PersistirGrupoPosicionesPartido', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604165620_PersistirFixture'
)
BEGIN
    CREATE TABLE [Fixtures] (
        [Id] int NOT NULL IDENTITY,
        [SemillaFixture] int NOT NULL,
        [FechaInicioTorneo] datetime2 NOT NULL,
        [MaxPartidosPorDia] int NOT NULL,
        [SeparacionEntreFechas] int NOT NULL,
        [EstaGenerado] bit NOT NULL,
        [CrucesGenerados] bit NOT NULL,
        CONSTRAINT [PK_Fixtures] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604165620_PersistirFixture'
)
BEGIN
    CREATE TABLE [FixtureEquipos] (
        [FixtureId] int NOT NULL,
        [EquipoNombre] nvarchar(60) NOT NULL,
        CONSTRAINT [PK_FixtureEquipos] PRIMARY KEY ([FixtureId], [EquipoNombre]),
        CONSTRAINT [FK_FixtureEquipos_Equipos_EquipoNombre] FOREIGN KEY ([EquipoNombre]) REFERENCES [Equipos] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FixtureEquipos_Fixtures_FixtureId] FOREIGN KEY ([FixtureId]) REFERENCES [Fixtures] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604165620_PersistirFixture'
)
BEGIN
    CREATE TABLE [FixtureEstadios] (
        [FixtureId] int NOT NULL,
        [EstadioNombre] nvarchar(80) NOT NULL,
        CONSTRAINT [PK_FixtureEstadios] PRIMARY KEY ([FixtureId], [EstadioNombre]),
        CONSTRAINT [FK_FixtureEstadios_Estadios_EstadioNombre] FOREIGN KEY ([EstadioNombre]) REFERENCES [Estadios] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FixtureEstadios_Fixtures_FixtureId] FOREIGN KEY ([FixtureId]) REFERENCES [Fixtures] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604165620_PersistirFixture'
)
BEGIN
    CREATE INDEX [IX_FixtureEquipos_EquipoNombre] ON [FixtureEquipos] ([EquipoNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604165620_PersistirFixture'
)
BEGIN
    CREATE INDEX [IX_FixtureEstadios_EstadioNombre] ON [FixtureEstadios] ([EstadioNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604165620_PersistirFixture'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260604165620_PersistirFixture', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604191145_BanderaEquipoMigracion'
)
BEGIN
    ALTER TABLE [Equipos] ADD [Bandera] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260604191145_BanderaEquipoMigracion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260604191145_BanderaEquipoMigracion', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260605192941_AgregarNotificaciones'
)
BEGIN
    CREATE TABLE [Notificaciones] (
        [Id] int NOT NULL IDENTITY,
        [Mensaje] nvarchar(500) NOT NULL,
        [FechaCreacion] datetime2 NOT NULL,
        [PeriodistaId] int NOT NULL,
        [Leida] bit NOT NULL,
        CONSTRAINT [PK_Notificaciones] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notificaciones_Usuarios_PeriodistaId] FOREIGN KEY ([PeriodistaId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260605192941_AgregarNotificaciones'
)
BEGIN
    CREATE INDEX [IX_Notificaciones_PeriodistaId] ON [Notificaciones] ([PeriodistaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260605192941_AgregarNotificaciones'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260605192941_AgregarNotificaciones', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260606200245_IncidenciaMigracion'
)
BEGIN
    CREATE TABLE [Incidencias] (
        [Id] int NOT NULL IDENTITY,
        [Tipo] nvarchar(30) NOT NULL,
        [EquipoNombre] nvarchar(60) NOT NULL,
        [Cantidad] int NOT NULL,
        [PartidoId] int NOT NULL,
        CONSTRAINT [PK_Incidencias] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Incidencias_Equipos_EquipoNombre] FOREIGN KEY ([EquipoNombre]) REFERENCES [Equipos] ([Nombre]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Incidencias_Partidos_PartidoId] FOREIGN KEY ([PartidoId]) REFERENCES [Partidos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260606200245_IncidenciaMigracion'
)
BEGIN
    CREATE INDEX [IX_Incidencias_EquipoNombre] ON [Incidencias] ([EquipoNombre]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260606200245_IncidenciaMigracion'
)
BEGIN
    CREATE INDEX [IX_Incidencias_PartidoId] ON [Incidencias] ([PartidoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260606200245_IncidenciaMigracion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260606200245_IncidenciaMigracion', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260606233055_AgregarIncidenciasYRankingPartido'
)
BEGIN
    ALTER TABLE [Partidos] ADD [RankingLocalAntes] int NOT NULL DEFAULT -1;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260606233055_AgregarIncidenciasYRankingPartido'
)
BEGIN
    ALTER TABLE [Partidos] ADD [RankingVisitanteAntes] int NOT NULL DEFAULT -1;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260606233055_AgregarIncidenciasYRankingPartido'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260606233055_AgregarIncidenciasYRankingPartido', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260608184015_AddNombreMotorSimulacionToFixture'
)
BEGIN
    ALTER TABLE [Fixtures] ADD [NombreMotorSimulacion] nvarchar(100) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260608184015_AddNombreMotorSimulacionToFixture'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260608184015_AddNombreMotorSimulacionToFixture', N'8.0.27');
END;
GO

COMMIT;
GO

