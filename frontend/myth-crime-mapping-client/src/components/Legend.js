import React from "react";
import "./Legend.css";

const Legend = ({ crimeTypes = [], isMarkerPanelVisible = true }) => {
  return (
    <div className={`legend-container ${isMarkerPanelVisible ? "open" : ""}`}>
      <strong className="legend-title">Преступления:</strong>
      <ul className="legend-list">
        {crimeTypes.map((type) => (
          <li className="legend-item" key={type.id}>
            <span
              className="legend-color"
              style={{ backgroundColor: type.color }}
            ></span>
            {type.title}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Legend;
