import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoinBankComponent } from './coin-bank.component';

describe('CoinBankComponent', () => {
  let component: CoinBankComponent;
  let fixture: ComponentFixture<CoinBankComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoinBankComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoinBankComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
