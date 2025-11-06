// components/SpaStatus.jsx
import React, { useEffect } from 'react';

const SpaStatus = () => {
  useEffect(() => {
    console.log('SPA navigation occurred — no full page reload');
  }, []);

  return (
    <div style={{ position: 'fixed', bottom: 10, right: 10, fontSize: '12px', color: 'green' }}>
      SPA Active 
    </div>
  );
};

export default SpaStatus;
