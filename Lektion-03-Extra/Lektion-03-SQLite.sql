-- UPPGIFT 9: ServerSecurityMapping
CREATE TABLE ServerSecurityMapping (
    ServerId INTEGER NOT NULL REFERENCES Servers(Id),
    ProfileId INTEGER NOT NULL REFERENCES SecurityProfiles(Id),

    PRIMARY KEY (ServerId, ProfileId)
);

-- UPPGIFT 10: Auditlogg med automatisk tidsstämpel
CREATE TABLE PipelineAuditLogs (
    Id INTEGER PRIMARY KEY,
    PipelineName TEXT NOT NULL,
    Status TEXT NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO PipelineAuditLogs (PipelineName, Status)
VALUES ('Build-Production', 'Success');