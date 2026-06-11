

-- DROP TABLE DA1Db.dbo.Equipos;

CREATE TABLE DA1Db.dbo.Equipos (
                                   Nombre nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                   Confederacion nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                   RankingFifa int NOT NULL,
                                   Bandera nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                                   CONSTRAINT PK_Equipos PRIMARY KEY (Nombre)
);


-- DA1Db.dbo.Estadios definition

-- Drop table

-- DROP TABLE DA1Db.dbo.Estadios;

CREATE TABLE DA1Db.dbo.Estadios (
                                    Nombre nvarchar(80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    Capacidad int NOT NULL,
                                    Ciudad nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    Descripcion nvarchar(400) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                                    CONSTRAINT PK_Estadios PRIMARY KEY (Nombre)
);


-- DA1Db.dbo.Fixtures definition

-- Drop table

-- DROP TABLE DA1Db.dbo.Fixtures;

CREATE TABLE DA1Db.dbo.Fixtures (
                                    Id int IDENTITY(1,1) NOT NULL,
                                    SemillaFixture int NOT NULL,
                                    FechaInicioTorneo datetime2 NOT NULL,
                                    MaxPartidosPorDia int NOT NULL,
                                    SeparacionEntreFechas int NOT NULL,
                                    EstaGenerado bit NOT NULL,
                                    CrucesGenerados bit NOT NULL,
                                    NombreMotorSimulacion nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'' NOT NULL,
                                    CONSTRAINT PK_Fixtures PRIMARY KEY (Id)
);


-- DA1Db.dbo.Grupos definition

-- Drop table

-- DROP TABLE DA1Db.dbo.Grupos;

CREATE TABLE DA1Db.dbo.Grupos (
                                  Id int IDENTITY(1,1) NOT NULL,
                                  Etiqueta nvarchar(2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                  CONSTRAINT PK_Grupos PRIMARY KEY (Id)
);


-- DA1Db.dbo.Usuarios definition

-- Drop table

-- DROP TABLE DA1Db.dbo.Usuarios;

CREATE TABLE DA1Db.dbo.Usuarios (
                                    Id int IDENTITY(1,1) NOT NULL,
                                    Nombre nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    Apellido nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    Email nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    FechaNacimiento datetime2 NOT NULL,
                                    Contrasena nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    Roles nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'' NOT NULL,
                                    CONSTRAINT PK_Usuarios PRIMARY KEY (Id)
);


-- DA1Db.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE DA1Db.dbo.[__EFMigrationsHistory];

CREATE TABLE DA1Db.dbo.[__EFMigrationsHistory] (
                                                   MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
    );


-- DA1Db.dbo.FixtureEquipos definition

-- Drop table

-- DROP TABLE DA1Db.dbo.FixtureEquipos;

CREATE TABLE DA1Db.dbo.FixtureEquipos (
                                          FixtureId int NOT NULL,
                                          EquipoNombre nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                          CONSTRAINT PK_FixtureEquipos PRIMARY KEY (FixtureId,EquipoNombre),
                                          CONSTRAINT FK_FixtureEquipos_Equipos_EquipoNombre FOREIGN KEY (EquipoNombre) REFERENCES DA1Db.dbo.Equipos(Nombre),
                                          CONSTRAINT FK_FixtureEquipos_Fixtures_FixtureId FOREIGN KEY (FixtureId) REFERENCES DA1Db.dbo.Fixtures(Id) ON DELETE CASCADE
);
CREATE NONCLUSTERED INDEX IX_FixtureEquipos_EquipoNombre ON DA1Db.dbo.FixtureEquipos (  EquipoNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DA1Db.dbo.FixtureEstadios definition

-- Drop table

-- DROP TABLE DA1Db.dbo.FixtureEstadios;

CREATE TABLE DA1Db.dbo.FixtureEstadios (
                                           FixtureId int NOT NULL,
                                           EstadioNombre nvarchar(80) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                           CONSTRAINT PK_FixtureEstadios PRIMARY KEY (FixtureId,EstadioNombre),
                                           CONSTRAINT FK_FixtureEstadios_Estadios_EstadioNombre FOREIGN KEY (EstadioNombre) REFERENCES DA1Db.dbo.Estadios(Nombre),
                                           CONSTRAINT FK_FixtureEstadios_Fixtures_FixtureId FOREIGN KEY (FixtureId) REFERENCES DA1Db.dbo.Fixtures(Id) ON DELETE CASCADE
);
CREATE NONCLUSTERED INDEX IX_FixtureEstadios_EstadioNombre ON DA1Db.dbo.FixtureEstadios (  EstadioNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DA1Db.dbo.LogsAuditoria definition

-- Drop table

-- DROP TABLE DA1Db.dbo.LogsAuditoria;

CREATE TABLE DA1Db.dbo.LogsAuditoria (
                                         Id int IDENTITY(1,1) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
                                         Accion nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                         UsuarioId int NOT NULL,
                                         CONSTRAINT PK_LogsAuditoria PRIMARY KEY (Id),
                                         CONSTRAINT FK_LogsAuditoria_Usuarios_UsuarioId FOREIGN KEY (UsuarioId) REFERENCES DA1Db.dbo.Usuarios(Id) ON DELETE CASCADE
);
CREATE NONCLUSTERED INDEX IX_LogsAuditoria_UsuarioId ON DA1Db.dbo.LogsAuditoria (  UsuarioId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DA1Db.dbo.Notificaciones definition

-- Drop table

-- DROP TABLE DA1Db.dbo.Notificaciones;

CREATE TABLE DA1Db.dbo.Notificaciones (
                                          Id int IDENTITY(1,1) NOT NULL,
                                          Mensaje nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                          FechaCreacion datetime2 NOT NULL,
                                          PeriodistaId int NOT NULL,
                                          Leida bit NOT NULL,
                                          CONSTRAINT PK_Notificaciones PRIMARY KEY (Id),
                                          CONSTRAINT FK_Notificaciones_Usuarios_PeriodistaId FOREIGN KEY (PeriodistaId) REFERENCES DA1Db.dbo.Usuarios(Id) ON DELETE CASCADE
);
CREATE NONCLUSTERED INDEX IX_Notificaciones_PeriodistaId ON DA1Db.dbo.Notificaciones (  PeriodistaId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DA1Db.dbo.Partidos definition

-- Drop table

-- DROP TABLE DA1Db.dbo.Partidos;

CREATE TABLE DA1Db.dbo.Partidos (
                                    Id int IDENTITY(1,1) NOT NULL,
                                    OrigenLocalId int NULL,
                                    OrigenVisitanteId int NULL,
                                    EstaBloqueado bit NOT NULL,
                                    TieneResultado bit NOT NULL,
                                    EsPorPerdedor bit NOT NULL,
                                    GolesLocalAnterior int NOT NULL,
                                    GolesVisitanteAnterior int NOT NULL,
                                    Codigo nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    Fecha datetime2 NOT NULL,
                                    Fase nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                    EquipoLocalNombre nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                                    EquipoVisitanteNombre nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                                    EstadioNombre nvarchar(80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                                    GrupoId int NULL,
                                    GolesLocal int NOT NULL,
                                    GolesVisitante int NOT NULL,
                                    VencedorNombre nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                                    RankingLocalAntes int DEFAULT -1 NOT NULL,
                                    RankingVisitanteAntes int DEFAULT -1 NOT NULL,
                                    CONSTRAINT PK_Partidos PRIMARY KEY (Id),
                                    CONSTRAINT FK_Partidos_Equipos_EquipoLocalNombre FOREIGN KEY (EquipoLocalNombre) REFERENCES DA1Db.dbo.Equipos(Nombre),
                                    CONSTRAINT FK_Partidos_Equipos_EquipoVisitanteNombre FOREIGN KEY (EquipoVisitanteNombre) REFERENCES DA1Db.dbo.Equipos(Nombre),
                                    CONSTRAINT FK_Partidos_Equipos_VencedorNombre FOREIGN KEY (VencedorNombre) REFERENCES DA1Db.dbo.Equipos(Nombre),
                                    CONSTRAINT FK_Partidos_Estadios_EstadioNombre FOREIGN KEY (EstadioNombre) REFERENCES DA1Db.dbo.Estadios(Nombre),
                                    CONSTRAINT FK_Partidos_Grupos_GrupoId FOREIGN KEY (GrupoId) REFERENCES DA1Db.dbo.Grupos(Id),
                                    CONSTRAINT FK_Partidos_Partidos_OrigenLocalId FOREIGN KEY (OrigenLocalId) REFERENCES DA1Db.dbo.Partidos(Id),
                                    CONSTRAINT FK_Partidos_Partidos_OrigenVisitanteId FOREIGN KEY (OrigenVisitanteId) REFERENCES DA1Db.dbo.Partidos(Id)
);
CREATE NONCLUSTERED INDEX IX_Partidos_EquipoLocalNombre ON DA1Db.dbo.Partidos (  EquipoLocalNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Partidos_EquipoVisitanteNombre ON DA1Db.dbo.Partidos (  EquipoVisitanteNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Partidos_EstadioNombre ON DA1Db.dbo.Partidos (  EstadioNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Partidos_GrupoId ON DA1Db.dbo.Partidos (  GrupoId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Partidos_OrigenLocalId ON DA1Db.dbo.Partidos (  OrigenLocalId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Partidos_OrigenVisitanteId ON DA1Db.dbo.Partidos (  OrigenVisitanteId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Partidos_VencedorNombre ON DA1Db.dbo.Partidos (  VencedorNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DA1Db.dbo.PosicionesGrupo definition

-- Drop table

-- DROP TABLE DA1Db.dbo.PosicionesGrupo;

CREATE TABLE DA1Db.dbo.PosicionesGrupo (
                                           Id int IDENTITY(1,1) NOT NULL,
                                           DiferenciaGoles int NOT NULL,
                                           EquipoNombre nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                           GrupoId int NOT NULL,
                                           Puntos int NOT NULL,
                                           GolesFavor int NOT NULL,
                                           GolesContra int NOT NULL,
                                           PosicionFinal int NOT NULL,
                                           CONSTRAINT PK_PosicionesGrupo PRIMARY KEY (Id),
                                           CONSTRAINT FK_PosicionesGrupo_Equipos_EquipoNombre FOREIGN KEY (EquipoNombre) REFERENCES DA1Db.dbo.Equipos(Nombre),
                                           CONSTRAINT FK_PosicionesGrupo_Grupos_GrupoId FOREIGN KEY (GrupoId) REFERENCES DA1Db.dbo.Grupos(Id)
);
CREATE NONCLUSTERED INDEX IX_PosicionesGrupo_EquipoNombre ON DA1Db.dbo.PosicionesGrupo (  EquipoNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_PosicionesGrupo_GrupoId ON DA1Db.dbo.PosicionesGrupo (  GrupoId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- DA1Db.dbo.Incidencias definition

-- Drop table

-- DROP TABLE DA1Db.dbo.Incidencias;

CREATE TABLE DA1Db.dbo.Incidencias (
                                       Id int IDENTITY(1,1) NOT NULL,
                                       Tipo nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                       EquipoNombre nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
                                       Cantidad int NOT NULL,
                                       PartidoId int NOT NULL,
                                       CONSTRAINT PK_Incidencias PRIMARY KEY (Id),
                                       CONSTRAINT FK_Incidencias_Equipos_EquipoNombre FOREIGN KEY (EquipoNombre) REFERENCES DA1Db.dbo.Equipos(Nombre),
                                       CONSTRAINT FK_Incidencias_Partidos_PartidoId FOREIGN KEY (PartidoId) REFERENCES DA1Db.dbo.Partidos(Id) ON DELETE CASCADE
);
CREATE NONCLUSTERED INDEX IX_Incidencias_EquipoNombre ON DA1Db.dbo.Incidencias (  EquipoNombre ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Incidencias_PartidoId ON DA1Db.dbo.Incidencias (  PartidoId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;