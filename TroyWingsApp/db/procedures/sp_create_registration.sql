USE troywings_db;

DROP PROCEDURE IF EXISTS sp_create_registration;
DELIMITER //
CREATE PROCEDURE sp_create_registration (
    IN p_name VARCHAR(80),
    IN p_father_name VARCHAR(80),
    IN p_date_of_birth DATE,
    IN p_contact_number VARCHAR(20),
    IN p_address VARCHAR(180),
    IN p_created_at_utc DATETIME
)
BEGIN
    INSERT INTO Registrations (
        Name,
        FatherName,
        DateOfBirth,
        ContactNumber,
        Address,
        CreatedAtUtc
    )
    VALUES (
        p_name,
        p_father_name,
        p_date_of_birth,
        p_contact_number,
        p_address,
        p_created_at_utc
    );
END //
DELIMITER ;
