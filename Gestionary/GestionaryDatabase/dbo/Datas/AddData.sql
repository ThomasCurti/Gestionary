INSERT INTO Roles (name) VALUES ('Admin'), ('Employee');

INSERT INTO Users (username, password_hash, role_id) VALUES ('admin', '262.VTDo6fAH/VN24A+4/EhA/A==.lQea9tC5uVWz336ajYNuztZsyA/L4EqjSxbqJl1er2U=', (SELECT id FROM ROLES Where name = 'Admin'));

INSERT INTO Types (name) VALUES ('Viande'), ('Poisson'), ('Fruit'), ('Légume'), ('Boisson');