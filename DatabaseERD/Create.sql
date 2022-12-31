-- This script was generated by a beta version of the ERD tool in pgAdmin 4.
-- Please log an issue at https://redmine.postgresql.org/projects/pgadmin4/issues/new if you find any bugs, including reproduction steps.
BEGIN;


CREATE TABLE IF NOT EXISTS public.users
(
    user_id integer NOT NULL GENERATED ALWAYS AS IDENTITY,
    username text NOT NULL,
    password text NOT NULL,
    PRIMARY KEY (user_id)
);

COMMENT ON TABLE public.users
    IS 'Table for Users';

CREATE TABLE IF NOT EXISTS public.cards
(
    card_id integer NOT NULL GENERATED ALWAYS AS IDENTITY,
    name text NOT NULL,
    damage real NOT NULL,
    card_type "char" NOT NULL,
    element_type "char" NOT NULL,
    PRIMARY KEY (card_id)
);

COMMENT ON TABLE public.cards
    IS 'All avaiable Cards in the Game';

CREATE TABLE IF NOT EXISTS public.users_cards
(
    card_id integer NOT NULL,
    user_id integer NOT NULL,
    PRIMARY KEY (card_id, user_id)
);

COMMENT ON TABLE public.users_cards
    IS 'Connection between User and Cards. Maps all cards a user has';

CREATE TABLE IF NOT EXISTS public.packages
(
    package_id integer NOT NULL GENERATED ALWAYS AS IDENTITY,
    price integer NOT NULL,
    active boolean NOT NULL,
    PRIMARY KEY (package_id)
);

COMMENT ON TABLE public.packages
    IS 'Stores all created packages';

CREATE TABLE IF NOT EXISTS public.packages_cards
(
    package_id integer NOT NULL,
    card_id integer NOT NULL,
    PRIMARY KEY (package_id, card_id)
);

COMMENT ON TABLE public.packages_cards
    IS 'Mapps cards to package';

CREATE TABLE IF NOT EXISTS public.decks
(
    deck_id integer NOT NULL GENERATED ALWAYS AS IDENTITY,
    user_id integer NOT NULL,
    PRIMARY KEY (deck_id)
);

COMMENT ON TABLE public.decks
    IS 'Deck of a user';

CREATE TABLE IF NOT EXISTS public.decks_cards
(
    deck_id integer NOT NULL,
    card_id integer NOT NULL,
    PRIMARY KEY (deck_id, card_id)
);

COMMENT ON TABLE public.decks_cards
    IS 'Map cards to a deck';

ALTER TABLE IF EXISTS public.users_cards
    ADD FOREIGN KEY (card_id)
    REFERENCES public.cards (card_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public.users_cards
    ADD FOREIGN KEY (user_id)
    REFERENCES public.users (user_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public.packages_cards
    ADD FOREIGN KEY (package_id)
    REFERENCES public.packages (package_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public.packages_cards
    ADD FOREIGN KEY (card_id)
    REFERENCES public.cards (card_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public.decks
    ADD FOREIGN KEY (user_id)
    REFERENCES public.users (user_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public.decks_cards
    ADD FOREIGN KEY (deck_id)
    REFERENCES public.decks (deck_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public.decks_cards
    ADD FOREIGN KEY (card_id)
    REFERENCES public.cards (card_id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;

END;