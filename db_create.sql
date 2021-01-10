-- Table: mtcg.User

-- DROP TABLE mtcg."User";

CREATE TABLE mtcg."User"
(
    "Id" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "Username" character varying(20) COLLATE pg_catalog."default" NOT NULL,
    "Password_Hash" bytea NOT NULL,
    "CurrentToken" character varying(200) COLLATE pg_catalog."default",
    "Coins" integer DEFAULT 100,
    "UnopenedPackages" character varying COLLATE pg_catalog."default",
    "Stack" character varying COLLATE pg_catalog."default",
    "Deck" character varying COLLATE pg_catalog."default",
    "Profile" character varying COLLATE pg_catalog."default",
    "Stats" character varying COLLATE pg_catalog."default",
    CONSTRAINT "User_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "User_Username_key" UNIQUE ("Username")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE mtcg."User"
    OWNER to "Robert";

GRANT ALL ON TABLE mtcg."User" TO "Robert";

GRANT ALL ON TABLE mtcg."User" TO swe1_mtcg;


-- Table: mtcg.Card

-- DROP TABLE mtcg."Card";

CREATE TABLE mtcg."Card"
(
    "Guid" uuid NOT NULL,
    "Type" character varying COLLATE pg_catalog."default" NOT NULL,
    "Name" character varying COLLATE pg_catalog."default" NOT NULL,
    "Damage" double precision NOT NULL,
    "Element" character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Card_pkey" PRIMARY KEY ("Guid"),
    CONSTRAINT "Card_Name_Type_key" UNIQUE ("Name", "Type")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE mtcg."Card"
    OWNER to "Robert";

GRANT ALL ON TABLE mtcg."Card" TO "Robert";

GRANT ALL ON TABLE mtcg."Card" TO swe1_mtcg;


-- Table: mtcg.Transactions

-- DROP TABLE mtcg."Transactions";

CREATE TABLE mtcg."Transactions"
(
    "Id" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "UserId" integer NOT NULL,
    "Description" character varying COLLATE pg_catalog."default",
    "Date" date DEFAULT '2020-12-01'::date,
    CONSTRAINT "Transactions_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Transactions_UserId_fkey" FOREIGN KEY ("UserId")
        REFERENCES mtcg."User" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE mtcg."Transactions"
    OWNER to swe1_mtcg;


-- Table: mtcg.TradingDeal

-- DROP TABLE mtcg."TradingDeal";

CREATE TABLE mtcg."TradingDeal"
(
    "Guid" uuid NOT NULL,
    "PublisherId" integer NOT NULL,
    "FullfillerId" integer,
    "CardId" uuid NOT NULL,
    "RequestedType" character varying COLLATE pg_catalog."default" NOT NULL,
    "MinimumDamage" double precision NOT NULL,
    "RequestedElement" character varying COLLATE pg_catalog."default" NOT NULL,
    "IsFullfilled" boolean,
    CONSTRAINT "TradingDeal_pkey" PRIMARY KEY ("Guid")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE mtcg."TradingDeal"
    OWNER to swe1_mtcg;


-- Table: mtcg.PackageType

-- DROP TABLE mtcg."PackageType";

CREATE TABLE mtcg."PackageType"
(
    "Id" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "Name" character varying COLLATE pg_catalog."default" NOT NULL,
    "Price" integer NOT NULL,
    CONSTRAINT "PackageType_pkey" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE mtcg."PackageType"
    OWNER to "Robert";

GRANT ALL ON TABLE mtcg."PackageType" TO "Robert";

GRANT ALL ON TABLE mtcg."PackageType" TO swe1_mtcg;


-- Table: mtcg.Package

-- DROP TABLE mtcg."Package";

CREATE TABLE mtcg."Package"
(
    "Id" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "PackageTypeId" integer NOT NULL,
    "Name" character varying COLLATE pg_catalog."default" NOT NULL,
    "Cards" character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Package_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Package_PackageTypeId_fkey" FOREIGN KEY ("PackageTypeId")
        REFERENCES mtcg."PackageType" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE mtcg."Package"
    OWNER to "Robert";

GRANT ALL ON TABLE mtcg."Package" TO "Robert";

GRANT ALL ON TABLE mtcg."Package" TO swe1_mtcg;