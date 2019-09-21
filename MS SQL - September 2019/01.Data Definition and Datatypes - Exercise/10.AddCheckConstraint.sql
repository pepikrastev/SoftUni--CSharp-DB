ALTER TABLE Users
ADD CONSTRAINT PasswordLength CHECK (LEN([Password]) >= 5)