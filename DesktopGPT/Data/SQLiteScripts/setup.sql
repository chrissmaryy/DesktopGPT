-- Create Chats Table
CREATE TABLE IF NOT EXISTS Chats (
    chat_id INTEGER PRIMARY KEY AUTOINCREMENT,
    chat_name TEXT NOT NULL DEFAULT ('New Chat')
);

-- Create Messages Table
CREATE TABLE IF NOT EXISTS Messages(
    message_id INTEGER PRIMARY KEY AUTOINCREMENT,
    chat_id INTEGER NOT NULL,
    model TEXT NOT NULL,
    role TEXT NOT NULL,
    content TEXT NOT NULL,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (chat_id) REFERENCES Chats(chat_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Create Token_Info Table
CREATE TABLE IF NOT EXISTS Token_Info(
    model TEXT PRIMARY KEY NOT NULL,
    max_tokens INTEGER NOT NULL,
    token_buffer INTEGER NOT NULL
);

-- Create Image_Requests Table
CREATE TABLE IF NOT EXISTS Image_Requests(
    image_request_id INTEGER PRIMARY KEY AUTOINCREMENT,
    chat_id INTEGER NOT NULL,
    prompt TEXT NOT NULL,
    size TEXT NOT NULL,
    quality TEXT NOT NULL,
    n INTEGER NOT NULL,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (chat_id) REFERENCES Chats(chat_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Create Images Table
CREATE TABLE IF NOT EXISTS Images(
    image_id INTEGER PRIMARY KEY AUTOINCREMENT,
    image_request_id INTEGER NOT NULL,
    image_b64 TEXT NOT NULL,
    FOREIGN KEY (image_request_id) REFERENCES Image_Requests(image_request_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Create User_Info Table
CREATE TABLE IF NOT EXISTS User_Info(
    user_id INTEGER PRIMARY KEY AUTOINCREMENT,
    api_key TEXT,
    temperature REAL NOT NULL,
    shortcut_key TEXT NOT NULL,
    shortcut_modifiers TEXT NOT NULL
);

INSERT OR IGNORE INTO Token_Info(
    model,
    max_tokens,
    token_buffer
)
VALUES
    ('gpt-4o', 8192, 1024),
    ('gpt-4o-mini', 4096, 512),
    ('o1', 16384, 2048),
    ('o1-mini', 8192, 1024),
    ('gpt-4-turbo', 128000, 8000),
    ('gpt-4', 32768, 4000),
    ('gpt-3.5-turbo', 16384, 2048)
;