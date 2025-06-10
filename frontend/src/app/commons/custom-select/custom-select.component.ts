import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  forwardRef,
  OnInit,
  HostListener,
  SimpleChanges,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { SelectService } from './select.service';

@Component({
  standalone: true,
  selector: 'app-custom-select',

  imports: [CommonModule],
  templateUrl: './custom-select.component.html',
  styleUrls: ['./custom-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomSelectComponent),
      multi: true,
    },
  ],
})
export class CustomSelectComponent implements ControlValueAccessor, OnInit {
  constructor(private selectService: SelectService) {}

  @Input() options: { value: string; label: string }[] = [];
  @Input() placeholder: string = 'Select...';

  value: string = '';
  selectedLabel: string = '';
  isOpen = false;

  private pendingValue: string | null = null;
  private onChange = (_: any) => {};
  private onTouched = () => {};

  ngOnInit() {
    this.tryUpdateLabel();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['options']) {
      this.tryUpdateLabel();
    }
  }

  writeValue(value: string): void {
    this.value = value?.toString();
    this.tryUpdateLabel();
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  toggleDropdown(): void {
    if (!this.isOpen) {
      this.selectService.registerOpen(this);
    }
    this.isOpen = !this.isOpen;
  }

  selectOption(option: { value: string; label: string }): void {
    this.value = option.value;
    this.selectedLabel = option.label;
    this.onChange(this.value);
    this.onTouched();
    this.isOpen = false;
  }

  close() {
    this.isOpen = false;
    this.selectService.clear(this);
  }

  private tryUpdateLabel() {
    if (!this.options || this.options.length === 0) {
      this.pendingValue = this.value;
      return;
    }

    const match = this.options.find(
      (opt) => opt.value.toString() === this.value
    );
    if (match) {
      this.selectedLabel = match.label;
      this.pendingValue = null;
    } else if (this.pendingValue) {
      const pendingMatch = this.options.find(
        (opt) => opt.value.toString() === this.pendingValue
      );
      if (pendingMatch) {
        this.value = pendingMatch.value;
        this.selectedLabel = pendingMatch.label;
        this.pendingValue = null;
      }
    }
  }

  @HostListener('document:click', ['$event'])
  handleOutsideClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.custom-select')) {
      this.isOpen = false;
    }
  }
}
