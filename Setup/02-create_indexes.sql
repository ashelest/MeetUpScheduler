CREATE INDEX IF NOT EXISTS idx_slots_start_end_date ON slots (start_date, end_date);
CREATE INDEX IF NOT EXISTS idx_languages ON sales_managers USING gin (languages);
CREATE INDEX IF NOT EXISTS idx_customer_ratings ON sales_managers USING gin (customer_ratings);
CREATE INDEX IF NOT EXISTS idx_products ON sales_managers USING gin (products);