INSERT INTO card_category (category_id) VALUES ('Goblin');
INSERT INTO card_category (category_id) VALUES ('Dragon');
INSERT INTO card_category (category_id) VALUES ('Wizzard');
INSERT INTO card_category (category_id) VALUES ('Ork');
INSERT INTO card_category (category_id) VALUES ('Knight');
INSERT INTO card_category (category_id) VALUES ('Kraken');
INSERT INTO card_category (category_id) VALUES ('FireElve');
INSERT INTO card_category (category_id) VALUES ('Troll');

INSERT INTO card_category (category_id) VALUES ('WaterSpell');
INSERT INTO card_category (category_id) VALUES ('FireSpell');
INSERT INTO card_category (category_id) VALUES ('RegularSpell');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('SmallGoblin', 'Goblin', 20, 'M', 'N');
INSERT INTO cards (name,category_id, damage, card_type, element_type) VALUES ('FireGoblin', 'Goblin', 25, 'M', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('WaterGoblin', 'Goblin', 25, 'M', 'W');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('SmallDragon', 'Dragon', 50, 'M', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('MediumDragon', 'Dragon', 75, 'M', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('BigDragon', 'Dragon', 100, 'M', 'F');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('StupidWizzard', 'Wizzard', 10, 'M', 'N');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('FireWizzard', 'Wizzard', 40, 'M', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('SmartWizzard', 'Wizzard', 80, 'M', 'N');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('RoyalKnight', 'Knight', 110, 'M', 'N');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('Knight', 'Knight', 60, 'M', 'N');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('SmallKraken', 'Kraken', 50, 'M', 'N');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('BigKraken', 'Kraken', 70, 'M', 'N');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('FireElve', 'FireElve', 40, 'M', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('FastFireElve', 'FireElve', 70, 'M', 'F');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('FireElve', 'FireElve', 40, 'M', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('FastFireElve', 'FireElve', 70, 'M', 'F');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('FatOrk', 'Ork', 50, 'M', 'N');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('Ork', 'Ork', 35, 'M', 'N');

INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('Troll', 'Troll', 35, 'S', 'N');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('WaterTroll', 'Troll', 50, 'S', 'W');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('FireTroll', 'Troll', 50, 'S', 'F');

--Spell Cards
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('SmallWaterSpell', 'WaterSpell', 25, 'S', 'W');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('BigWaterSpell', 'WaterSpell', 55, 'S', 'W');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('SmallRegularSpell', 'RegularSpell', 30, 'S', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('BigRegularSpell', 'RegularSpell', 50, 'S', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('SmallFireSpell', 'FireSpell', 30, 'S', 'F');
INSERT INTO cards (name, category_id, damage, card_type, element_type) VALUES ('BigFireSpell', 'FireSpell', 50, 'S', 'F');


INSERT INTO users (username, password, coins) VALUES ('admin', 'admin', 10000);
INSERT INTO users_stats (user_Id, username, coins_spent, battles_played, wins, win_rate) VALUES (1, 'admin', 0, 0, 0, 0);
INSERT INTO decks (user_Id) VALUES (1);
INSERT INTO users_cards(card_id, user_id, count) VALUES (1, 1, 1);
INSERT INTO decks_cards (deck_id, card_id) VALUES (1, 1);
INSERT INTO decks_cards (deck_id, card_id) VALUES (1, 3);
INSERT INTO decks_cards (deck_id, card_id) VALUES (1, 5);
INSERT INTO decks_cards (deck_id, card_id) VALUES (1, 10);

INSERT INTO users (username, password, coins) VALUES ('John', 'Wick', 500);
INSERT INTO users_stats (user_Id, username, coins_spent, battles_played, wins, win_rate) VALUES (2, 'Test', 0, 0, 0, 0);
INSERT INTO decks (user_Id) VALUES (2);
INSERT INTO users_cards(card_id, user_id, count) VALUES (1, 2, 1);
INSERT INTO users_cards(card_id, user_id, count) VALUES (5, 2, 1);
INSERT INTO users_cards(card_id, user_id, count) VALUES (10, 2, 1);
INSERT INTO users_cards(card_id, user_id, count) VALUES (13, 2, 1);
