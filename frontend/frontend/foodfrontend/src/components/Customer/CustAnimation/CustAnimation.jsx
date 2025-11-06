import React, { useState, useEffect } from 'react';
 
const SVG_VIEWBOX_SIZE = 1000;
const FADE_DURATION = 0.7; 
const STAGGER_DELAY = 0.1; 

const TOTAL_FADING_WORDS = 26;
 

const CASCADE_TIME_MS = (TOTAL_FADING_WORDS * STAGGER_DELAY + FADE_DURATION) * 1000;

const CYCLE_INTERVAL = CASCADE_TIME_MS + 500; 
 

const tailwindToSvgStyle = (config) => {
  const styles = {
    
    fill: config.color.includes('red') ? '#DC2626' : (config.color.includes('gray-600') ? '#4B5563' : '#9CA3AF'),

    fontSize: `${config.size}px`,
    fontFamily: 'Inter, sans-serif',
  
    fontWeight: config.weight === 'font-black' ? 900 : config.weight === 'font-extrabold' ? 800 : config.weight === 'font-bold' ? 700 : 600,

    transition: `opacity ${FADE_DURATION}s ease-in-out`,
  };
  return styles;
};
 

const ALL_WORDS = [
  'Delicious', 'Tasty', 'Yummy', 'Savory', 'Spicy', 'Sweet', 'Fresh', 'Homemade', 'Crispy', 'Flavorful',
  'Superfast', 'Irresistible', 'Authentic', 'Mouthwatering', 'Zesty',
  'स्वादिष्ट', 'लज़ीज़', 'मीठा', 'घर का बना', 'करारा',
  'സുவையான', 'രുചിയാന', 'വേഗമാന', 'எதிர்க்கமுடியாத', 'உண்மையான', 'തിയ്യനി', // Corrected a couple of non-English/non-Hindi words
];
 

const NON_FADING_CONFIG = [
  { id: 'meals-main', text: 'MEALS', size: 45, color: 'text-red-600', x: 500, y: 200, weight: 'font-black' },
];
 

const FADING_CONFIG = [

  { id: 'word-01', text: ALL_WORDS[0], size: 25, color: 'text-red-600', x: 500, y: 150, weight: 'font-semibold' }, // Was 600 -> +100
  { id: 'word-02', text: ALL_WORDS[1], size: 19, color: 'text-gray-400', x: 650, y: 220, weight: 'font-normal' }, // Was 350 -> -100
  { id: 'word-03', text: ALL_WORDS[2], size: 19, color: 'text-gray-400', x: 650, y: 160, weight: 'font-normal' }, // Was 550 -> +100
  { id: 'word-04', text: ALL_WORDS[3], size: 25, color: 'text-gray-600', x: 500, y: 270, weight: 'font-bold' }, // Was 500 -> Keep
  { id: 'word-05', text: ALL_WORDS[4], size: 19, color: 'text-gray-400', x: 320, y: 250, weight: 'font-normal' }, // Was 420 -> -100
  { id: 'word-06', text: ALL_WORDS[5], size: 19, color: 'text-gray-400', x: 570, y: 90, weight: 'font-normal' }, // Was 580 -> +100
 

  { id: 'word-07', text: ALL_WORDS[6], size: 21, color: 'text-gray-400', x: 330, y: 140, weight: 'font-normal' }, // Was 300 -> -100
  { id: 'word-08', text: ALL_WORDS[7], size: 27, color: 'text-red-600', x: 820, y: 100, weight: 'font-semibold' }, // Was 720 -> +100
  { id: 'word-09', text: ALL_WORDS[8], size: 21, color: 'text-gray-400', x: 400, y: 350, weight: 'font-normal' }, // Was 300 -> -100
  { id: 'word-10', text: ALL_WORDS[9], size: 27, color: 'text-gray-600', x: 650, y: 340, weight: 'font-bold' }, // Was 720 -> +100
  { id: 'word-11', text: ALL_WORDS[10], size: 19, color: 'text-gray-600', x: 250, y: 160, weight: 'font-normal' }, // Was 350 -> -100
  { id: 'word-12', text: ALL_WORDS[11], size: 29, color: 'text-red-600', x: 760, y: 200, weight: 'font-semibold' }, // Was 660 -> +100
  { id: 'word-13', text: ALL_WORDS[12], size: 19, color: 'text-gray-400', x: 250, y: 350, weight: 'font-normal' }, // Was 350 -> -100
  { id: 'word-14', text: ALL_WORDS[13], size: 17, color: 'text-gray-400', x: 650, y: 120, weight: 'font-normal' }, // Was 650 -> +100
 

  { id: 'word-15', text: ALL_WORDS[14], size: 21, color: 'text-red-600', x: 180, y: 120, weight: 'font-semibold' }, // Was 300 -> -100
  { id: 'word-16', text: ALL_WORDS[15], size: 27, color: 'text-gray-600', x: 430, y: 100, weight: 'font-bold' }, // Was 700 -> +100
  { id: 'word-17', text: ALL_WORDS[16], size: 21, color: 'text-red-600', x: 200, y: 260, weight: 'font-semibold' }, // Was 300 -> -100
  { id: 'word-18', text: ALL_WORDS[17], size: 27, color: 'text-gray-600', x: 800, y: 350, weight: 'font-bold' }, // Was 700 -> +100
  { id: 'word-19', text: ALL_WORDS[18], size: 21, color: 'text-gray-400', x: 300, y: 90, weight: 'font-normal' }, // Was 400 -> -100
  { id: 'word-20', text: ALL_WORDS[19], size: 17, color: 'text-gray-400', x: 700, y: 90, weight: 'font-normal' }, // Was 600 -> +100
 

  { id: 'word-21', text: ALL_WORDS[20], size: 27, color: 'text-red-600', x: 230, y: 200, weight: 'font-semibold' }, // Was 250 -> -100
  { id: 'word-22', text: ALL_WORDS[21], size: 19, color: 'text-gray-400', x: 150, y: 320, weight: 'font-normal' }, // Was 250 -> -100
  { id: 'word-23', text: ALL_WORDS[22], size: 17, color: 'text-gray-400', x: 850, y: 150, weight: 'font-normal' }, // Was 750 -> +100
  { id: 'word-24', text: ALL_WORDS[23], size: 27, color: 'text-gray-600', x: 750, y: 280, weight: 'font-bold' }, // Was 750 -> +100
  { id: 'word-25', text: ALL_WORDS[24], size: 21, color: 'text-red-600', x: 500, y: 320, weight: 'font-semibold' }, // Was 500 -> Keep
  { id: 'word-26', text: ALL_WORDS[25], size: 17, color: 'text-gray-400', x: 300, y: 300, weight: 'font-normal' }, // Was 400 -> -100
];
 
 

const CustAnimation = () => {

  const [isFadingOut, setIsFadingOut] = useState(true);
 

  useEffect(() => {
  
    const interval = setInterval(() => {
      setIsFadingOut(prev => !prev);
    }, CYCLE_INTERVAL); 
 
    return () => clearInterval(interval);
  }, []);
 
 
  const getWordStyle = (index) => {
   
    const opacity = isFadingOut ? 0.1 : 1;
 
    const delayFactor = STAGGER_DELAY;
    const totalWords = FADING_CONFIG.length;
 
    let transitionDelay;
 
    if (isFadingOut) {
     
      transitionDelay = `${index * delayFactor}s`;
    } else {
      
      transitionDelay = `${(totalWords - 1 - index) * delayFactor}s`;
    }
 
    return { opacity, transitionDelay };
  };
 
  return (
    <div>
      <div>
       
 
        
        <svg
          viewBox={`0 0 ${SVG_VIEWBOX_SIZE} ${SVG_VIEWBOX_SIZE}`}
          className="w-full h-auto"
          preserveAspectRatio="xMidYMid meet"
        >
         
          {FADING_CONFIG.map((word, index) => {
            const { opacity, transitionDelay } = getWordStyle(index);
 
            return (
              <text
                key={word.id}
                x={word.x}
                y={word.y}
                style={{
                  ...tailwindToSvgStyle(word),
                  opacity: opacity,
                  transitionDelay: transitionDelay,
                }}
                
                textAnchor="middle"
                dominantBaseline="central"
              >
                {word.text}
              </text>
            );
          })}

          {NON_FADING_CONFIG.map((word) => (
            <text
              key={word.id}
              x={word.x}
              y={word.y}
              style={{
                ...tailwindToSvgStyle(word),
                
                opacity: 1,
              }}
              textAnchor="middle"
              dominantBaseline="central"
            >
              {word.text}
            </text>
          ))}
        </svg>
      </div>
    </div>
  );
};
 
export default CustAnimation;
 
 