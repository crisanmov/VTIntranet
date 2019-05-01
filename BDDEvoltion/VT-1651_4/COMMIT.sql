
CREATE TABLE [dbo].[tbluserstags](
idUserTag INT IDENTITY NOT NULL,
idUser INT,
idTag INT,
userTagActive BIT,
CONSTRAINT PK_tbluserstags_idUserTag PRIMARY KEY (idUserTag)
)

ALTER TABLE tbluserstags
ADD CONSTRAINT FK_tbluserstags_tblusers
FOREIGN KEY (idUser) REFERENCES tblusers(idUser);

ALTER TABLE tbluserstags
ADD CONSTRAINT FK_tbluserstags_tbltags
FOREIGN KEY (idTag) REFERENCES tbltags(idTag);