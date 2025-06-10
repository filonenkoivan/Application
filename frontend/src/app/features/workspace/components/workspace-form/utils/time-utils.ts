export const formatDateToDateTimeString = (date: Date) => {
  return (
    date.getFullYear() +
    '-' +
    String(date.getMonth() + 1).padStart(2, '0') +
    '-' +
    String(date.getDate()).padStart(2, '0') +
    'T00:00:00'
  );
};

export function formatTimeTo24(timeStr: string): string {
  if (!timeStr) return '00:00:00';

  let hour = Number(timeStr);
  if (isNaN(hour) || hour < 0 || hour > 23) {
    return '00:00:00';
  }
  const hourStr = hour.toString().padStart(2, '0');
  return `${hourStr}:00:00`;
}

export function getDaysInMonth(
  month: number,
  year: number,
  isStart: boolean = false,
  minDay: number = 1,
  compareMonth?: number,
  compareYear?: number
): number[] {
  const today = new Date();
  const totalDays = new Date(year, month, 0).getDate();

  let startDay = 1;

  if (
    isStart &&
    year === today.getFullYear() &&
    month === today.getMonth() + 1
  ) {
    startDay = today.getDate();
  }

  if (
    !isStart &&
    compareMonth !== undefined &&
    compareYear !== undefined &&
    year === compareYear &&
    month === compareMonth
  ) {
    startDay = Math.max(startDay, minDay);
  }

  return Array.from(
    { length: totalDays - startDay + 1 },
    (_, i) => i + startDay
  );
}

export function getMonths(
  selectedYear: number,
  isStart: boolean
): { value: number; name: string }[] {
  const monthNames = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];
  const today = new Date();
  const currentYear = today.getFullYear();
  const currentMonth = today.getMonth() + 1;

  let startMonth = 1;
  if (isStart && selectedYear === currentYear) {
    startMonth = currentMonth;
  }

  return Array.from({ length: 12 - startMonth + 1 }, (_, i) => {
    const monthIndex = startMonth + i - 1;
    return {
      value: monthIndex + 1,
      name: monthNames[monthIndex],
    };
  });
}

export function formatTime(hour24: number): string {
  const period = hour24 >= 12 ? 'PM' : 'AM';
  const hour12 = hour24 % 12 === 0 ? 12 : hour24 % 12;
  return `${hour12}:00 ${period}`;
}

export function parseHourFromTimeString(timeStr: string): number {
  if (!timeStr) return -1;
  const [time, period] = timeStr.split(' ');
  let [hour] = time.split(':').map(Number);
  if (period === 'PM' && hour !== 12) hour += 12;
  if (period === 'AM' && hour === 12) hour = 0;
  return hour;
}
