import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeAgo',
  standalone: true
})
export class TimeAgoPipe implements PipeTransform {
  transform(value: any): string {
    if (value) {
      const seconds = Math.floor((+new Date() - +new Date(value)) / 1000);
      if (seconds < 29) {
        return 'Just now';
      }

      const intervals: { [key: string]: number } = {
        year: 31536000,
        month: 2592000,
        week: 604800,
        day: 86400,
        hour: 3600,
        minute: 60,
        second: 1,
      };

      let counter: number;
      let unit: string;

      for (const i in intervals) {
        counter = Math.floor(seconds / intervals[i]);
        unit = counter === 1 ? i : i + 's';
        if (counter > 0) {
          return `${counter} ${unit} ago`;
        }
      }
    }

    return value;
  }
}
