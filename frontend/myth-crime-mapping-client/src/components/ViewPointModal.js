import React, { useState } from "react";
import { Modal, Form } from "react-bootstrap";

const ViewPointModal = ({ point, onHide }) => {

  const onShow = () => {
    if(!!point) {
      return true;
    }
    else {
      return false;
    }
  }

  return (
    <>
      <Modal show={onShow} onHide={onHide} size="lg">
        <Modal.Header closeButton>
            <Modal.Title>Просмотр преступления</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p className="mb-1">
            Вид преступления: {point.title}
          </p>

          <p className="mb-1">
            Преступник
          </p>

          <div className="mb-1 form-control">
          <p>Фамилия: {point.wantedPersonSurname}</p>
          <p>Имя: {point.wantedPersonName}</p>
          {point.wantedPersonPatronymic && (
            <p>Отчество: {point.wantedPersonPatronymic}</p>
          )}
          <p>Дата рождения: {point.wantedPersonBirthDate?.split("T")[0] || ""}</p>
          </div>

          <p className="mb-1">
            Место совершения преступления: {point.location}
          </p>

          <p className="mb-1">
            Дата совершения преступления: {point.crimeDate?.split("T")[0] || ""}
          </p>

          {point.description && (
            <div>
              <p>Описание:</p>
              <p className="form-control">{point.description}</p>
            </div>
          )}

        </Modal.Body>
      </Modal>
    </>
    
  );
};

export default ViewPointModal;
