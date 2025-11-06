import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "./Sidebar.css";
import Profile from "./Profile";
import ProfileImage from '../../assets/profile.png';
import HistoryImage from '../../assets/History.png';
import OrdersImage from '../../assets/IncomingOrders.png';




function Sidebar() {
  const [agentName, setAgentName] = useState("");
  const [error, setError] = useState(null);
  const [isActive, setIsActive] = useState(true);
  const navigate = useNavigate();

  const token = localStorage.getItem("token");
  const agentId = localStorage.getItem("userId");

  useEffect(() => {
    const fetchAgentName = async () => {
      try {
        const response = await fetch(
          `https://localhost:7025/api/DeliveryAgent/GetAgentName?id=${agentId}`,
          {
            method: "GET",
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }

        const text = await response.text();
        setAgentName(text);
      } catch (err) {
        setError(err.message);
      }
    };

    if (token && agentId) {
      fetchAgentName();
    } else {
      setError("Missing token or agent ID");
    }
  }, [token, agentId]);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("role");
    navigate("/");
    alert("You are now logged out");
  };

  const handleToggleDuty = async () => {
    try {
      const response = await fetch(
        `https://localhost:7025/api/DeliveryAgent/updateAgentStatus?AgentId=${agentId}&status=${!isActive}`,
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      if (!response.ok) {
        throw new Error("Failed to update duty status");
      }

      const result = await response.text();
      console.log(result);
      setIsActive((prev) => !prev);
      alert(result);
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="sidebar-agent">
      <h3>{error ? `Error: ${error}` : `Hi, ${agentName || "Loading..."}`}</h3>
      <nav>
        <Link to="/agent/profile"><img src={ProfileImage} className="sidebar-icon"/>Profile</Link>
        <Link to="/agent/order-history"><img src={HistoryImage} className="sidebar-icon"/>Order History</Link>
        <Link to="/agent/active-orders"><img src={OrdersImage} className="sidebar-icon"/>Active Orders</Link>
      </nav>
      <div className="action-buttons">
        <button onClick={handleToggleDuty} className="duty-button">
          {isActive ? "Go Off Duty" : "Go On Duty"}
        </button>
        <button onClick={handleLogout} className="logout-button">
          Logout
        </button>
      </div>
    </div>
  );
}

export default Sidebar;