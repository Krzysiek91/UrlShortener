-- Terminal command to create sqlite database file (Embeded DBMS): sqlite3 UrlShortenerDB.db

PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "URLs" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"URL"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("Id" AUTOINCREMENT)
);

CREATE UNIQUE INDEX index_url 
ON URLs (URL);

COMMIT;
